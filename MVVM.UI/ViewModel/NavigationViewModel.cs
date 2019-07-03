using MVVM.Model;
using MVVM.UI.Data;
using MVVM.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.ViewModel
{
   public class NavigationViewModel :ViewModelBase ,  INavigationViewModel
    {
        public NavigationViewModel(ILookUpDataService friendLookUpService,IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _LookUpService = friendLookUpService;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterSaveFriendEvent>().Subscribe(AfterFriendSaved);
        }

        private void AfterFriendSaved(AfterSavedEventArgs SavedFriend)
        {
          var lookup =  Friends.Single(Friend => Friend.Id == SavedFriend.Id);
            lookup.FirstName = SavedFriend.DisplayName; 
           

        }
        public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }
        public async Task LoadAsync()
        {
            var lookup = await _LookUpService.GetFriendLookUpAsync();
            Friends.Clear();
            foreach(var look in lookup)
            {
                Friends.Add(new NavigationItemViewModel (look.Id , look.FirstName));
            }
        }

        private IEventAggregator _eventAggregator;
        private ILookUpDataService _LookUpService;

      

        private NavigationItemViewModel _selectedFriend;

        public NavigationItemViewModel SelectedFriend
        {
            get { return _selectedFriend; }
            set { _selectedFriend = value;
               OnPropertyChanged();
                if(_selectedFriend != null) {
                    _eventAggregator.GetEvent<OpenFriendEvent>().Publish(_selectedFriend.Id);
                        }
            }
        }

    }
}
