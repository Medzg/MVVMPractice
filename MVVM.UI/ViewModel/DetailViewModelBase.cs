using MVVM.UI.Event;
using MVVM.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        private string _title;
        protected readonly IMessageDialogService MessageDialogeService;
        public DetailViewModelBase(IEventAggregator EventAggregator,IMessageDialogService messageDialogService)

        {
            eventAggregator = EventAggregator;
            MessageDialogeService = messageDialogService; 
            CloseDetailCommand = new DelegateCommand(OnCloseDetailViewExecute);
            SaveCommand = new DelegateCommand(OnSaveExecute, onSaveCanExecute);
            DeleteCommand = new DelegateCommand(onDeleteExecute);
        }

        
        

        protected virtual void RaiseCollectionSavedEvent()
        {


            eventAggregator.GetEvent<AfterCollectionSavedEvent>().Publish(new AfterCollectionSavedEventArgs
            {
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void OnCloseDetailViewExecute()
        {
            if (HasChanged)
            {
                var result = MessageDialogeService.ShowOkCancelDialog("Are you sure you want to leave There some changes need to be saved", "Warning");
                if(result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            eventAggregator.GetEvent<AfterDetailCloseEvent>().Publish(new AfterDetailCloseArgs
            {

                Id = this.Id,
                ViewModelName = this.GetType().Name
            });
        }

        private int _id;

        public int Id
        {
            get { return _id; }
           protected set { _id = value; }
        }
        public string Title { get {

                return _title;            } set {
                _title = value;
                OnPropertyChanged(); } }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CloseDetailCommand { get; private set; }
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

        public abstract Task LoadAsync(int id);

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
                ViewModelName = this.GetType().Name
            });
        }

        protected async Task OnSaveOptimisticConcurnceyAsyc(Func<Task>SaveFunc,Action AfterSaveAction)
        {
            try
            {


                await SaveFunc();

            }
            catch (DbUpdateConcurrencyException ex)
            {

                var databasevalue = ex.Entries.Single().GetDatabaseValues();
                if (databasevalue == null)
                {

                    MessageDialogeService.ShowInfoDialog("this entity was been deleted By another user");
                    RaiseDetailDeletedEvent(Id);
                    return;
                }
                var res = MessageDialogeService.ShowOkCancelDialog("The entity has been changed in by another meantime by someone else. Click ok to save your changes anyway, click Cancel to reload the entity from the database", "Question");
                if (res == MessageDialogResult.Ok)
                {

                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await SaveFunc();
                }
                else
                {
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }


            AfterSaveAction();


        }


    }
}
