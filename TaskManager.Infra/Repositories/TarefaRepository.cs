using TaskManager.Infra.Context;
using TaskManager.Infra.Entity;
using TaskManager.Infra.Repositories.Interfaces;

namespace TaskManager.Infra.Repositories
{
    public class TarefaRepository : RepositoryGeneric<Tarefa>, ITarefaRepository
    {
        private ClientContext _appContext => (ClientContext)_context;

        public TarefaRepository(ClientContext context) : base(context)
        { }
    }
}
