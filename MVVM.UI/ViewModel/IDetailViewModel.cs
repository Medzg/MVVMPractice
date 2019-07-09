using System.Threading.Tasks;

namespace MVVM.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool HasChanged { get; }
        int Id { get; }
    }
}