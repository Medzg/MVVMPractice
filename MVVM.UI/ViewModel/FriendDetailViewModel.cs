using MVVM.Model;
using MVVM.UI.Data;
using MVVM.UI.Event;
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
        private IFriendDataService _dataService;
        private IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        public FriendDetailViewModel(IFriendDataService friendDataService,IEventAggregator eventAggregator)
        {

            _dataService = friendDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendEvent>().Subscribe(OnOpenFriendAsync);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExectute);
        }

        public async Task LoadAsync(int FriendId)
        {

            var friend = await _dataService.GetByIdAsync(FriendId);
            this.Friend = new FriendWrapper(friend);
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

        public ICommand SaveCommand { get; }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Friend.Model);
            _eventAggregator.GetEvent<AfterSaveFriendEvent>().Publish(new AfterSavedEventArgs
            {
                Id = this.Friend.Id,
                DisplayName = this.Friend.FirstName + " " + this.Friend.LastName

            });
        }

        private bool OnSaveCanExectute()
        {

            return true;
        }

        private async void OnOpenFriendAsync(int FriendId)
        {
            await LoadAsync(FriendId);
           
        }

    }
}
