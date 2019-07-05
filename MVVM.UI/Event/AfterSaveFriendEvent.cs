using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Event
{
   public class AfterSaveFriendEvent:PubSubEvent<AfterSavedEventArgs>    {
    }

    public class AfterSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string ViewModelNew { get; set; }
    }
}
