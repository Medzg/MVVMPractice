using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;
using MVVM.UI.Wrapper;

namespace MVVM.UI.Data.Repositories
{
    public interface IFriendDataRepository : IGenericRepository<Friend>
    {
       
        void RemovePhoneNumber(FriendPhoneNumber model);
        Task<bool> HasMeetingAsync(int friendID);
    }
}