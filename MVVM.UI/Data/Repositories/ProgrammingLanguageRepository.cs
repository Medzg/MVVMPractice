using MVVM.DataAccess;
using MVVM.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Data.Repositories
{
    public class ProgrammingLanguageRepository : GenericRepository<ProgrammingLanguage, FriendDbContext>, IProgrammingLanguageRepository
    {
       public ProgrammingLanguageRepository(FriendDbContext context) : base(context)
        {
        }

        public async Task<bool> IsReferencedByFriendAsync(int programmingLanguageId)
        {
            return await Context.Friends.AsNoTracking().AnyAsync(f => f.FavoriteLangugeId == programmingLanguageId);
        }
    }
}
