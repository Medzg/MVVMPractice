using MVVM.DataAccess;
using MVVM.Model;
using MVVM.UI.Wrapper;
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

        public void Add(Friend friend)
        {
            _context.Friends.Add(friend);
        }

        public void Delete(Friend model)
        {
            _context.Friends.Remove(model);
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
