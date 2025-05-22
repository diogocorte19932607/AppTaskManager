using TaskManager.Infra.Context;
using TaskManager.Infra.Repository;

namespace TaskManager.Infra.UnitofWork
{
    public interface IUnitofWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;
        ClientContext Context { get; }
        int Save();
        Task<int> SaveAsync();
    }
}
