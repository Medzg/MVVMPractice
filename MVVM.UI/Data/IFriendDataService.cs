using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;

namespace MVVM.UI.Data
{
    public interface IFriendDataService
    {
        Task<Friend> GetByIdAsync(int FriendId);
        Task SaveAsync(Friend friend);
    }
}