using Microsoft.EntityFrameworkCore;

using TaskManager.Infra.Entity;

namespace TaskManager.Infra.Context
{
    public partial class ClientContext : DbContext
    {

        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        public DbSet<Tarefa> tarefas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

    }
}
