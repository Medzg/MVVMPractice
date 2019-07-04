using MVVM.UI.Event;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.UI.ViewModel
{
   public class NavigationItemViewModel :ViewModelBase
    {

        public NavigationItemViewModel( int id , string displayMember,IEventAggregator eventAggregator)
        {
            Id = id;
           FirstName = displayMember;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
            _eventAggregator = eventAggregator;        }

        private void OnOpenFriendDetailView()
        {
            _eventAggregator.GetEvent<OpenFriendEvent>().Publish(Id);
        }

        public int Id { get;}
        private string _firstName;
        public ICommand OpenFriendDetailViewCommand { get; }

        private IEventAggregator _eventAggregator;

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
