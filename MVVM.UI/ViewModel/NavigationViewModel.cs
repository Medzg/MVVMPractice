using MVVM.Model;
using MVVM.UI.Data;
using MVVM.UI.Data.Lookups;
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
        public NavigationViewModel(ILookUpDataService friendLookUpService,IEventAggregator eventAggregator,IMeetingLookUpDataService meetingLookUpDataService)
        {
            _eventAggregator = eventAggregator;
            _LookUpService = friendLookUpService;
            _meetingLookUpDataService = meetingLookUpDataService; 
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterSaveFriendEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDeleteEvent>().Subscribe(AfterFriendDeleted);
        }

        private void AfterFriendDeleted(AfterDeleteEventArgs args)
        {
            switch(args.ViewModelName){


                case nameof(FriendDetailViewModel):
                    AfterFriendDeleted(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterFriendDeleted(Meetings, args);
                    break;
            }
         
        }

        private void AfterFriendDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDeleteEventArgs args)
        {
            var item = items.SingleOrDefault(fr => fr.Id == args.Id);
            if (item != null)
            {
               items.Remove(item);

            }
        }

        private void AfterDetailSaved(AfterSavedEventArgs SavedFriend)
        {
            switch (SavedFriend.ViewModelName) {
                case nameof(FriendDetailViewModel)  :
                    AfterDetailSaved(Friends, SavedFriend);
            break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, SavedFriend);
                    break;

            }
    }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterSavedEventArgs obj)
        {
            var lookup = items.SingleOrDefault(Friend => Friend.Id == obj.Id);
            if (lookup == null)
            {
                items.Add(new NavigationItemViewModel(obj.Id, obj.DisplayName, _eventAggregator, obj.ViewModelName));
            }
            else
            {
                lookup.FirstName = obj.DisplayName;

            }
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; private set; }
        public async Task LoadAsync()
        {
            var lookup = await _LookUpService.GetFriendLookUpAsync();
            Friends.Clear();
            foreach(var look in lookup)
            {
                Friends.Add(new NavigationItemViewModel (look.Id , look.FirstName,_eventAggregator,nameof(FriendDetailViewModel) ));
            }
            lookup = await _meetingLookUpDataService.GetMeetingLookUpAsync();
            Meetings.Clear();
            foreach (var look in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(look.Id, look.FirstName, _eventAggregator, nameof(MeetingDetailViewModel)));
            }
        }

        private IEventAggregator _eventAggregator;
        private ILookUpDataService _LookUpService;
        private IMeetingLookUpDataService _meetingLookUpDataService;
    }
}
