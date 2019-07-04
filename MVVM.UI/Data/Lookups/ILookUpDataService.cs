using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;

namespace MVVM.UI.Data.Lookups
{
    public interface ILookUpDataService
    {
        Task<IEnumerable<LookUpItem>> GetFriendLookUpAsync();
    }
}