using MVVM.DataAccess;
using MVVM.Model;
using MVVM.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Data.Repositories
{
    public  class FriendDataRepository : GenericRepository<Friend,FriendDbContext> ,IFriendDataRepository
    {
        

        public FriendDataRepository(FriendDbContext context) :base(context)
        {
         
        }
        public override async Task<Friend> GetByIdAsync(int FriendId)
        {

               return await Context.Friends.Include(f=>f.PhoneNumbers).SingleAsync(fr=> fr.Id == FriendId);
       
        }

      
        public async Task<bool> HasMeetingAsync(int friendID)
        {
            return await Context.Meetings.AsNoTracking().Include(m => m.Friends).AnyAsync(m => m.Friends.Any(f => f.Id == friendID));

        }
        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.PhoneNumbers.Remove(model);
        }



      
    }
}
