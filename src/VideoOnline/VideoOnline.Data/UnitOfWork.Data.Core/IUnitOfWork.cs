namespace VideoOnline.Data.UnitOfWork.Data.Core
{
    /// <summary>
    /// 工作单元
    /// 提供一个保存方法，它可以对调用曾公开，为了减少连库次数
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 将操作提交到数据库
        /// </summary>
        void Save();

        /// <summary>
        /// 是否不提交到数据库，这只是具体的repository类中的SaveChanges方法里用到的
        /// 默认为false，即默认为提交到数据库
        /// </summary>
        bool IsNotSubmit { get; set; }
    }

    /// <summary>
    /// 工作单元
    /// 对泛型类型的支持
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork<T> : IUnitOfWork where T : class { }
}
