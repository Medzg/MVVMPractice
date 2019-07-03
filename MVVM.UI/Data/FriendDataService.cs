using MVVM.DataAccess;
using MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Data
{
  public  class FriendDataService : IFriendDataService
    {
        private Func<FriendDbContext> _contextCreator;

        public FriendDataService(Func<FriendDbContext>contextCreator)
        {
            _contextCreator = contextCreator; 
        }
        public async Task<Friend> GetByIdAsync(int FriendId)
        {
           using(var ctx =  _contextCreator())
            {
               return await ctx.Friends.AsNoTracking().SingleAsync(fr=> fr.Id == FriendId);
            }

        }

        public async Task SaveAsync(Friend friend)
        {
            using (var ctx = _contextCreator())
            {

                ctx.Friends.Attach(friend);
                ctx.Entry(friend).State = EntityState.Modified;
               await ctx.SaveChangesAsync();
            }
        }
    }
}
