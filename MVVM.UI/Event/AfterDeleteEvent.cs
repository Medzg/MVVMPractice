using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Event
{
    class AfterDeleteEvent: PubSubEvent<AfterDeleteEventArgs>
    {
    }  
    public class AfterDeleteEventArgs
    {

        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
