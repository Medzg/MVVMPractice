using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;

namespace MVVM.UI.Data
{
    public interface ILookUpDataService
    {
        Task<IEnumerable<LookUpItem>> GetFriendLookUpAsync();
    }
}