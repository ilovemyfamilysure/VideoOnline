using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using VideoOnline.Data.UnitOfWork.Data.Core;

namespace VideoOnline.Data.EntityFrameWork.Data.Core
{
    public class DbContextRepository<TEntity> : ICompleteRepository<TEntity> where TEntity : class
    {
        protected EFContext _db { get; private set; }

        public IUnitOfWork IUnitOfWork
        {
            get
            {
                return iUnitOfWork;
            }

            set
            {
                iUnitOfWork = value;
            }
        }

        private IUnitOfWork iUnitOfWork;

        public DbContextRepository(IUnitOfWork db)
        {
            IUnitOfWork = db;
            _db = (EFContext)db;
        }

        public DbContextRepository() { }

        #region IRepository<T> 成员

        public virtual void Insert(TEntity item)
        {
            _db.Entry(item);
            _db.Set<TEntity>().Add(item);
            SaveChanges();
        }

        public virtual void Delete(TEntity item)
        {
            _db.Set<TEntity>().Attach(item);
            _db.Set<TEntity>().Remove(item);
            SaveChanges();
        }

        public virtual void Update(TEntity item)
        {
            _db.Set<TEntity>().Attach(item);
            _db.Entry(item).State = EntityState.Modified;
            SaveChanges();
        }

        public void Update(Expression<Action<TEntity>> entity)
        {
            TEntity newEntity = typeof(TEntity).GetConstructor(Type.EmptyTypes).Invoke(null) as TEntity;//建立指定类型的实例
            List<string> propertyNameList = new List<string>();
            MemberInitExpression param = entity as MemberInitExpression;
            foreach (var item in param.Bindings)
            {
                string propertyName = item.Member.Name;
                object propertyValue;
                var memberAssignment = item as MemberAssignment;
                if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                {
                    propertyValue = (memberAssignment.Expression as ConstantExpression).Value;
                }
                else
                {
                    propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                }
                typeof(TEntity).GetProperty(propertyName).SetValue(newEntity, propertyValue, null);
                propertyNameList.Add(propertyName);
            }
            _db.Set<TEntity>().Attach(newEntity);
            _db.Configuration.ValidateOnSaveEnabled = false;
            var ObjectStateEntry = ((IObjectContextAdapter)_db).ObjectContext.ObjectStateManager.GetObjectStateEntry(newEntity);
            propertyNameList.ForEach(x => ObjectStateEntry.SetModifiedProperty(x.Trim()));
            SaveChanges();
        }

        public IQueryable<TEntity> GetModelList()
        {
            return _db.Set<TEntity>();
        }
        
        public IQueryable<TEntity> GetModelList(Expression<Func<TEntity, bool>> func)
        {
            return _db.Set<TEntity>().Where(func);
        }
        
        public IQueryable<TEntity> GetModelList(Expression<Func<TEntity, bool>> func, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderd, int pageSize, int index, ref int count)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().Where(func);
            count = query.Count();
            if (orderd != null)
                return orderd(query).Skip((index - 1) * pageSize).Take(pageSize);

            return query.Skip((index - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region IExtensionRepository<T> 成员

        public virtual void Insert(IEnumerable<TEntity> item)
        {
            item.ToList().ForEach(i =>
            {
                Insert(i);//不提交
            });
        }

        public virtual void Delete(IEnumerable<TEntity> item)
        {
            item.ToList().ForEach(i =>
            {
                Delete(i);
            });
        }

        public virtual void Update(IEnumerable<TEntity> item)
        {
            item.ToList().ForEach(i =>
            {
                Update(i);
            });
        }

        public TEntity Find(params object[] id)
        {
            return _db.Set<TEntity>().Find(id);
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// 根据工作单元的IsNotSubmit的属性，去判断是否提交到数据库
        /// 一般地，在多个repository类型进行组合时，这个IsNotSubmit都会设为true，即不马上提交，
        /// 而对于单个repository操作来说，它的值不需要设置，使用默认的false，将直接提交到数据库，这也保证了操作的原子性。
        /// </summary>
        protected void SaveChanges()
        {
            if (!IUnitOfWork.IsNotSubmit)
                IUnitOfWork.Save();
        }

        /// <summary>
        ///  计数更新,与SaveChange()是两个SQL链接，走分布式事务
        ///  子类可以根据自己的逻辑，去复写
        ///  tableName:表名
        ///  param:索引0为主键名，1表主键值，2为要计数的字段，3为增量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="param">参数列表，索引0为主键名，1表主键值，2为要计数的字段，3为增量</param>
        protected virtual void UpdateForCount(string tableName, params object[] param)
        {
            string sql = "update [" + tableName + "] set [{2}]=ISNULL([{2}],0)+{3} where [{0}]={1}";
            List<object> listParasm = new List<object>
            {
                param[0],
                param[1],
                param[2],
                param[3],
            };
            _db.Database.ExecuteSqlCommand(string.Format(sql, listParasm.ToArray()));
        }
        #endregion
    }
}
