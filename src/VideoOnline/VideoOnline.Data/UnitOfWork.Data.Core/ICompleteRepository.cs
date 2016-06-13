namespace VideoOnline.Data.UnitOfWork.Data.Core
{
    /// <summary>
    /// 完整的数据操作接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICompleteRepository<T> : IRepository<T>,IExtensionRepository<T> where T : class{ }
}
