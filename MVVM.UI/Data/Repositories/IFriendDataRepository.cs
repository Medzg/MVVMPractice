using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;
using MVVM.UI.Wrapper;

namespace MVVM.UI.Data
{
    public interface IFriendDataRepository
    {
        Task<Friend> GetByIdAsync(int FriendId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Delete(Friend model);
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}