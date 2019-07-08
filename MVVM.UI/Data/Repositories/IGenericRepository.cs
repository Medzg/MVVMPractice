using System.Threading.Tasks;

namespace MVVM.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int Id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Delete(T model);

    }
}