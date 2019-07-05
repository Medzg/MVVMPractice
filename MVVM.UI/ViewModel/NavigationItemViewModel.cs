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

        public NavigationItemViewModel( int id , string displayMember,IEventAggregator eventAggregator,string detailViewModelName)
        {
            _detailViewModelName = detailViewModelName;
            Id = id;
           FirstName = displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
            _eventAggregator = eventAggregator;        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailEvent>().Publish(new OpenDetailEventArgs { Id = Id, ViewModelName = _detailViewModelName });
        }

        private string _detailViewModelName;

        public int Id { get;}
        private string _firstName;
        public ICommand OpenDetailViewCommand { get; }

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
