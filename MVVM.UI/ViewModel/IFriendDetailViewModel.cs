using System.Threading.Tasks;

namespace MVVM.UI.ViewModel
{
    public interface IFriendDetailViewModel
    {
        Task LoadAsync(int FriendId);
        bool HasChanged { get; }
    }
}