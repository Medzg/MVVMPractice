using MVVM.Model;
using MVVM.UI.Data;
using MVVM.UI.Event;
using MVVM.UI.View.Services;
using MVVM.UI.Wrapper;
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
   public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IMessageDialogService _messageDialogService;
        private IFriendDataRepository _dataRepository;
        private IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        public FriendDetailViewModel(IFriendDataRepository friendDataRepository,IEventAggregator eventAggregator,IMessageDialogService messageDialogService)
        {

            _messageDialogService = messageDialogService;

            _dataRepository = friendDataRepository;
            _eventAggregator = eventAggregator;
         
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExectute);
            DeleteCommand = new DelegateCommand(onDelete);
            
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

            var friend = FriendId.HasValue? await _dataRepository.GetByIdAsync(FriendId.Value):CreateNewFriend();
            this.Friend = new FriendWrapper(friend);

            this.Friend.PropertyChanged += (s, e) =>
             {
                 if (!HasChanged)
                 {
                     HasChanged = _dataRepository.HasChanges();
                 }

                 if(e.PropertyName == nameof(Friend.HasErrors))
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


        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

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

            return Friend!=null && !Friend.HasErrors && HasChanged;
        }

        

    }
}
