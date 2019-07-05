using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.Event
{
   public class OpenDetailEvent : PubSubEvent<OpenDetailEventArgs>
    {
    }
    public class OpenDetailEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }

    }
}
