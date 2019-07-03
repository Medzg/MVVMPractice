using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.ViewModel
{
   public class NavigationItemViewModel :ViewModelBase
    {

        public NavigationItemViewModel( int id , string displayMember)
        {
            Id = id;
           FirstName = displayMember; 
        }
        public int Id { get;}
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set {
                _firstName = value;
                OnPropertyChanged();
            }
        }

    }
}
