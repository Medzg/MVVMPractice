using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;

namespace MVVM.UI.Data
{
    public interface IFriendDataRepository
    {
        Task<Friend> GetByIdAsync(int FriendId);
        Task SaveAsync();
        bool HasChanges();
    }
}