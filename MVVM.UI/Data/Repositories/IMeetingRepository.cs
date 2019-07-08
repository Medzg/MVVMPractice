using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;

namespace MVVM.UI.Data.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
       Task<List<Friend>> GetAllFriendsAsync();
    }
}