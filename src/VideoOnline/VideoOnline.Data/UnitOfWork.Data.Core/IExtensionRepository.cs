using System;
using System.Collections.Generic;

namespace VideoOnline.Data.UnitOfWork.Data.Core
{
    /// <summary>
    /// 扩展的Repository操作规范
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IExtensionRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="collection"></param>
        void Insert(IEnumerable<TEntity> collection);

        /// <summary>
        /// 修改集合
        /// </summary>
        /// <param name="collection"></param>
        void Update(IEnumerable<TEntity> collection);

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="collection"></param>
        void Delete(IEnumerable<TEntity> collection);

        /// <summary>
        /// 扩展更新方法，只对ef支持
        /// </summary>
        /// <param name="entity"></param>
        void Update(System.Linq.Expressions.Expression<Action<TEntity>> entity);
    }
}
