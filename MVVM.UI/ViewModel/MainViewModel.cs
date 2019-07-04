using MVVM.Model;
using MVVM.UI.Data;
using MVVM.UI.Event;
using MVVM.UI.View.Services;
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
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel,Func<IFriendDetailViewModel> friendDetailViewModelCreator,IEventAggregator eventAggregator , IMessageDialogService messageDialogeService )
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogeService;
            NavigationViewModel = navigationViewModel;
            _friendDetailViewModel = friendDetailViewModelCreator;
            _eventAggregator.GetEvent<OpenFriendEvent>().Subscribe(OnOpenFriendAsync);
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
        }

        private void OnCreateNewFriendExecute()
        {
            OnOpenFriendAsync(null);
        }

        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public INavigationViewModel NavigationViewModel { get; }
        public ICommand CreateNewFriendCommand { get; }

        private Func<IFriendDetailViewModel> _friendDetailViewModel;

        private IFriendDetailViewModel friendDetailViewModel;

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get { return friendDetailViewModel; }
            set { friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }



        private async void OnOpenFriendAsync(int? FriendId)
        {
            if(FriendDetailViewModel != null && FriendDetailViewModel.HasChanged)
            {
                var result = _messageDialogService.ShowOkCancelDialog("you made a change ! are you sure to leave , all changes goona lost.", "Warning");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }

            }
          FriendDetailViewModel =  _friendDetailViewModel();
            await FriendDetailViewModel.LoadAsync(FriendId);

        }
    }
}
