using Autofac.Features.Indexed;
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
        public MainViewModel(INavigationViewModel navigationViewModel,IIndex<string,IDetailViewModel> detailViewModelCreator ,IEventAggregator eventAggregator , IMessageDialogService messageDialogeService )
        {
            
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogeService;
            NavigationViewModel = navigationViewModel;
            _detailViewModelCreator = detailViewModelCreator;
            _eventAggregator.GetEvent<OpenDetailEvent>().Subscribe(OnOpenDetailViewAsync);
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            _eventAggregator.GetEvent<AfterDeleteEvent>().Subscribe(OnDelete);
        }

        private void OnDelete(AfterDeleteEventArgs args)
        {
            DetailViewModel = null;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailViewAsync(new OpenDetailEventArgs { ViewModelName =  viewModelType.Name});
        }

      
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public INavigationViewModel NavigationViewModel { get; }

        private IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public ICommand CreateNewDetailCommand { get; }

  

        private IDetailViewModel _detailViewModel;

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set { _detailViewModel = value;
                OnPropertyChanged();
            }
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }



        private async void OnOpenDetailViewAsync(OpenDetailEventArgs  args)
        {
            if(DetailViewModel != null && DetailViewModel.HasChanged)
            {
                var result = _messageDialogService.ShowOkCancelDialog("you made a change ! are you sure to leave , all changes goona lost.", "Warning");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }

            }

            DetailViewModel = _detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id);

        }
    }
}
