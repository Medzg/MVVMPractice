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
            DetailViewModels = new ObservableCollection<IDetailViewModel>();
            NavigationViewModel = navigationViewModel;
            _detailViewModelCreator = detailViewModelCreator;
            _eventAggregator.GetEvent<OpenDetailEvent>().Subscribe(OnOpenDetailViewAsync);
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            _eventAggregator.GetEvent<AfterDeleteEvent>().Subscribe(OnDelete);
            _eventAggregator.GetEvent<AfterDetailCloseEvent>().Subscribe(AfterDetailClose);
         
        }

        private void AfterDetailClose(AfterDetailCloseArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void OnDelete(AfterDeleteEventArgs args)
        {
            RemoveDetailViewModel(args.Id,args.ViewModelName);

        }

        private void RemoveDetailViewModel(int id, string DetailViewModel)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == id && vm.GetType().Name == DetailViewModel);
            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }
        private int nextNewItemId = 0;

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailViewAsync(new OpenDetailEventArgs { Id = nextNewItemId --,ViewModelName =  viewModelType.Name});
        }

      
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public INavigationViewModel NavigationViewModel { get; }

        private IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public ICommand CreateNewDetailCommand { get; }
        public ObservableCollection<IDetailViewModel> DetailViewModels { get;}


        private IDetailViewModel _selectedDetailViewModel;

        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set { _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }



        private async void OnOpenDetailViewAsync(OpenDetailEventArgs  args)
        {
           var detailViewModel =  DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);
            if(detailViewModel  == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }
            SelectedDetailViewModel = detailViewModel;
           
           

        }
    }
}
