using System.Data.Entity;
using System.Threading.Tasks;

namespace MVVM.UI.Data.Repositories
{
    public class GenericRepository<TEntity,TContext> : IGenericRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context)
        {

            this.Context = context;
        }

    

        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public void Delete(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public virtual async Task<TEntity> GetByIdAsync(int Id)
        {
           return await Context.Set<TEntity>().FindAsync(Id);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public  async Task SaveAsync()
        {
            await Context.SaveChangesAsync();

        }
    }
}
