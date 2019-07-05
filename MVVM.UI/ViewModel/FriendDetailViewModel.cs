using MVVM.Model;
using MVVM.UI.Data;
using MVVM.UI.Data.Lookups;
using MVVM.UI.Event;
using MVVM.UI.View.Services;
using MVVM.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.UI.ViewModel
{
   public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IMessageDialogService _messageDialogService;
        private IFriendDataRepository _dataRepository;
        private IEventAggregator _eventAggregator;
        private ILookUpProgramingLangagueDataService _lookUpProgramingLangagueDataService;
        private FriendPhoneWrapper _selectedFriendPhone;

        public ObservableCollection<LookUpItem> ProgramingLangagues { get; }

        private FriendWrapper _friend;
        public FriendDetailViewModel(IFriendDataRepository friendDataRepository,IEventAggregator eventAggregator,IMessageDialogService messageDialogService, ILookUpProgramingLangagueDataService programingLangagueDataService)
        {

            _messageDialogService = messageDialogService;

            _dataRepository = friendDataRepository;
            _eventAggregator = eventAggregator;
            _lookUpProgramingLangagueDataService = programingLangagueDataService;
            ProgramingLangagues = new ObservableCollection<LookUpItem>();
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExectute);
            DeleteCommand = new DelegateCommand(onDelete);
            AddPhoneNumber = new DelegateCommand(OnAddNewPhoneNumber);
            DeletePhoneNumber = new DelegateCommand(OnDeletePhoneNumber, OnDeleteDeletePhoneCanExcute);

            PhoneNumbers = new ObservableCollection<FriendPhoneWrapper>();

            
        }

        private void OnAddNewPhoneNumber()
        {
            var newNumber = new FriendPhoneWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        private void OnDeletePhoneNumber()
        {
            SelectedFriendPhone.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            _dataRepository.RemovePhoneNumber(SelectedFriendPhone.Model);
            PhoneNumbers.Remove(SelectedFriendPhone);
            SelectedFriendPhone = null;
            HasChanged = _dataRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnDeleteDeletePhoneCanExcute()
        {
            return SelectedFriendPhone != null; 
        }

        private async void onDelete()
        {

            var result = _messageDialogService.ShowOkCancelDialog($"Are you sure you want to delete {Friend.FirstName} {Friend.LastName}","Question");
            if(result == MessageDialogResult.Ok) { 
            _dataRepository.Delete(Friend.Model);
            _eventAggregator.GetEvent<AfterDeleteEvent>().Publish(Friend.Id);
           await _dataRepository.SaveAsync();
        }
        }

        public async Task LoadAsync(int? FriendId)
        {

            var friend = FriendId.HasValue ? await _dataRepository.GetByIdAsync(FriendId.Value) : CreateNewFriend();
            InitilizeFriend(friend);
            InitilizeFriendPhoneNumber(friend.PhoneNumbers);
            await LoadProgramingLanguages();
        }

        private void InitilizeFriendPhoneNumber(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach(var phoneNumber in PhoneNumbers)
            {
                phoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;

            }
            PhoneNumbers.Clear();
            foreach(var phone in phoneNumbers)
            {
                var wrapper = new FriendPhoneWrapper(phone);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;

            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanged)
            {
                HasChanged = _dataRepository.HasChanges();
            }
            if(e.PropertyName == nameof(FriendWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitilizeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);

            this.Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanged)
                {
                    HasChanged = _dataRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgramingLanguages()
        {
            ProgramingLangagues.Clear();
            ProgramingLangagues.Add(new NullLookUpItem { FirstName = "Please chose Favorite Programing Language" });
            var Prglangs = await _lookUpProgramingLangagueDataService.GetProgramingLangagueAsync();
            foreach (var lookup in Prglangs)
            {
                ProgramingLangagues.Add(lookup);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _dataRepository.Add(friend);
            return friend; 

               
        }

        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        private bool _HasChanged;

        public bool HasChanged
        {
            get { return _HasChanged; }
            set { 
                if(_HasChanged != value) {
                    _HasChanged = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public FriendPhoneWrapper SelectedFriendPhone
        {
            get { return _selectedFriendPhone; }
            set
            {
                _selectedFriendPhone = value;
                OnPropertyChanged();
                ((DelegateCommand)DeletePhoneNumber).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand AddPhoneNumber { get;}
        public ICommand DeletePhoneNumber { get; }

        private async void OnSaveExecute()
        {
            await _dataRepository.SaveAsync();
            HasChanged = _dataRepository.HasChanges();
            _eventAggregator.GetEvent<AfterSaveFriendEvent>().Publish(new AfterSavedEventArgs
            {
                Id = this.Friend.Id,
                DisplayName = this.Friend.FirstName + " " + this.Friend.LastName

            });
            
        }

        private bool OnSaveCanExectute()
        {

            return Friend != null && !Friend.HasErrors && PhoneNumbers.All(pn => !pn.HasErrors)
                && HasChanged;
        }

        public ObservableCollection<FriendPhoneWrapper> PhoneNumbers { get;}



    }
}
