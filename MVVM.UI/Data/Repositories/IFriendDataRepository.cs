using System;
using System.Collections.Generic;
using MVVM.Model;
using MVVM.UI.Wrapper;

namespace MVVM.UI.Data
{
    public interface IFriendDataRepository : IGenericRepository<Friend>
    {
       
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}