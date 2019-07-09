using System.Threading.Tasks;
using MVVM.DataAccess;
using MVVM.Model;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace MVVM.UI.Data.Repositories
{
    public   class MeetingRepository : GenericRepository<Meeting, FriendDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendDbContext friendDb) : base(friendDb)
        {
          
        }

        public async override Task<Meeting> GetByIdAsync(int Id)
        {
         return await   Context.Meetings.Include(m => m.Friends).SingleAsync(m => m.Id == Id);
        }
        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await Context.Set<Friend>().ToListAsync();
        }

        public async Task ReloadFrienAsync(int FriendId)
        {
            var DbEntityEntry = Context.ChangeTracker.Entries<Friend>().SingleOrDefault(db => db.Entity.Id == FriendId);
            if(DbEntityEntry != null)
            {
               await DbEntityEntry.ReloadAsync();
            }
        }
    }
}
