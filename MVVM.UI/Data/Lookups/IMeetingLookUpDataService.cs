using System.Collections.Generic;
using System.Threading.Tasks;
using MVVM.Model;

namespace MVVM.UI.Data.Lookups
{
    public interface IMeetingLookUpDataService
    {
        Task<IEnumerable<LookUpItem>> GetMeetingLookUpAsync();
    }
}