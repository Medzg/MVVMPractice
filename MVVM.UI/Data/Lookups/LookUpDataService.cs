using MVVM.DataAccess;
using MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Data.Lookups
{
     public class LookUpDataService : ILookUpDataService, ILookUpProgramingLangagueDataService, IMeetingLookUpDataService
    {
        private Func<FriendDbContext> _context;

        public LookUpDataService(Func<FriendDbContext> DbContext)
        {
            _context = DbContext; 

        }
        public async Task<IEnumerable<LookUpItem>> GetFriendLookUpAsync()
        {
            using(var ctx = _context())
            {
                return await ctx.Friends.AsNoTracking().Select(Fr => new LookUpItem
                {
                    Id = Fr.Id,
                    FirstName = Fr.FirstName  + " " + Fr.LastName
                }).ToListAsync();

            }
        }

        public async Task<IEnumerable<LookUpItem>> GetProgramingLangagueAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking().Select(PL => new LookUpItem
                {

                    Id = PL.Id,
                    FirstName = PL.Name
                }).ToListAsync();

            }


        }

        public async Task<IEnumerable<LookUpItem>> GetMeetingLookUpAsync()
        {
            using (var ctx = _context())
            {
                return  await ctx.Meetings.AsNoTracking().Select(PL => new LookUpItem
                {

                    Id = PL.Id,
                    FirstName = PL.Title
                }).ToListAsync();

            }


        }




    }
    }

