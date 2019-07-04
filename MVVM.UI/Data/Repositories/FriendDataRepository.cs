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
  public  class FriendDataRepository : IFriendDataRepository
    {
        private FriendDbContext _context;

        public FriendDataRepository(FriendDbContext context)
        {
            _context = context; 
        }
        public async Task<Friend> GetByIdAsync(int FriendId)
        {
          
      
               return await _context.Friends.SingleAsync(fr=> fr.Id == FriendId);
       

        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
           
        

               
               await _context.SaveChangesAsync();
           
        }
    }
}
