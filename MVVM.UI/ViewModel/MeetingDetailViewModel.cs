using MVVM.Model;
using MVVM.UI.Data.Repositories;
using MVVM.UI.View.Services;
using MVVM.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.UI.ViewModel
{
  public  class MeetingDetailViewModel :DetailViewModelBase , IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private IMessageDialogService _messageDialogSerivce;
        private MeetingWrapper _meeting;

        private Friend _SelectedAvailableFriend;
        private Friend _SelectedAddedFriend;
        private List<Friend> _friends;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,IMessageDialogService messageDialogService, IMeetingRepository meetingRepository  ) :base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogSerivce = messageDialogService;
            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(onAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(onRemoveFriendExecute, OnRemoveFriendCanExecute);

        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private void onRemoveFriendExecute()
        {
            var FriendToAdd = SelectedAddedFriend;
            Meeting.Model.Friends.Remove(FriendToAdd);
            AddedFriends.Remove(FriendToAdd);
            AvailableFriends.Add(FriendToAdd);
            HasChanged = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void onAddFriendExecute()
        {
            var FriendToAdd = SelectedAvailableFriend;
            Meeting.Model.Friends.Add(FriendToAdd);
            AddedFriends.Add(FriendToAdd);
            AvailableFriends.Remove(FriendToAdd);
            HasChanged = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

        }

        public MeetingWrapper Meeting
        {
            get
            {
                return _meeting; 
            }
             private set
            {
                _meeting = value;
                OnPropertyChanged();

            }
        }

        public ICommand AddFriendCommand { get; }
        public ICommand RemoveFriendCommand { get; }

        public ObservableCollection<Friend> AddedFriends { get; }

        public ObservableCollection<Friend> AvailableFriends { get;  }

        public Friend SelectedAvailableFriend { get {

                return _SelectedAvailableFriend; } set {
                _SelectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
                ; } }

        public Friend SelectedAddedFriend { get {return _SelectedAddedFriend; } set {

                _SelectedAddedFriend = value;
                OnPropertyChanged();
                    ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged(); } }
        public async override Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
            ? await _meetingRepository.GetByIdAsync(meetingId.Value) : createNewMeeting();

            InitializeMeeting(meeting);
            _friends = await _meetingRepository.GetAllFriendsAsync();
            SetupPickList();
        }

        private void SetupPickList()
        {
            var meetingFriendsIds = Meeting.Model.Friends.Select(f => f.Id);
            var addedFriends = _friends.Where(f => meetingFriendsIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availbleFriends = _friends.Except(addedFriends).OrderBy(f => f.FirstName);
            AddedFriends.Clear();
            AvailableFriends.Clear();
            foreach(var addedFriend in addedFriends)
            {

                AddedFriends.Add(addedFriend);
            }
            foreach(var availableFriend in availbleFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {

                if (!HasChanged)
                {

                    HasChanged = _meetingRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if(Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
        }

        private Meeting createNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected override void onDeleteExecute()
        {
            var result = _messageDialogSerivce.ShowOkCancelDialog($"Do you really want to delete it{Meeting.Title} ", "Warning");
            if(result == MessageDialogResult.Ok)
            {
                _meetingRepository.Delete(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool onSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanged;
        }

        protected  async override void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanged = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}
