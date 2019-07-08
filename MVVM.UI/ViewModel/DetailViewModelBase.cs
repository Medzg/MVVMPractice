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
   public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges ;
        private readonly IEventAggregator eventAggregator;
        public DetailViewModelBase(IEventAggregator EventAggregator)

        {
            eventAggregator = EventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, onSaveCanExecute);
            DeleteCommand = new DelegateCommand(onDeleteExecute);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public bool HasChanged {
            get { return _hasChanges; }
            set { _hasChanges = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }


        protected abstract void OnSaveExecute();
        protected abstract bool onSaveCanExecute();
        protected abstract void onDeleteExecute();

        public abstract Task LoadAsync(int? id);

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {

            eventAggregator.GetEvent<AfterDeleteEvent>().Publish(new AfterDeleteEventArgs
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int Modelid,string displayname) {


            eventAggregator.GetEvent<AfterSaveFriendEvent>().Publish(new AfterSavedEventArgs
            {

                Id = Modelid,
                DisplayName = displayname,
                ViewModelNew = this.GetType().Name
            });
        }


    }
}
