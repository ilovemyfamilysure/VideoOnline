using System;
using System.Linq;
using System.Linq.Expressions;

namespace VideoOnline.Data.UnitOfWork.Data.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加实体数据并提交到数据服务器
        /// </summary>
        /// <param name="item"></param>
        void Insert(TEntity item);

        /// <summary>
        /// 修改实体数据并提交到数据服务器
        /// </summary>
        /// <param name="item"></param>
        void Update(TEntity item);

        /// <summary>
        /// 移除实体数据并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item"></param>
        void Delete(TEntity item);

        /// <summary>
        /// 根据主键得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(params object[] id);

        /// <summary>
        /// 获得实体的所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetModelList();

        /// <summary>
        /// 根据条件查询实体数据
        /// </summary>
        /// <param name="func">lambda表达式的条件语句</param>
        /// <returns></returns>
        IQueryable<TEntity> GetModelList(Expression<Func<TEntity, bool>> func);

        /// <summary>
        /// 根据条件查询分页实体数据
        /// </summary>
        /// <param name="func">lambda表达式的条件语句</param>
        /// <param name="orderd">lambda表达式的排序语句</param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="index">当前页码</param>
        /// <param name="count">所有数据条数(ref参数)</param>
        /// <returns></returns>
        IQueryable<TEntity> GetModelList(Expression<Func<TEntity, bool>> func, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderd, int pageSize, int index, ref int count);
    }
}
