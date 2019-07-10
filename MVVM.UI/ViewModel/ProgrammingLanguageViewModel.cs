using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVM.Model;
using MVVM.UI.Data.Repositories;
using MVVM.UI.View.Services;
using MVVM.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace MVVM.UI.ViewModel
{
    public class ProgrammingLanguageViewModel : DetailViewModelBase
    {
        private IProgrammingLanguageRepository _programmingLanguageRepository;
        private ProgrammingLanguageWrapper _selectedProgrammingLanguage; 
        public ProgrammingLanguageViewModel(IEventAggregator EventAggregator, IMessageDialogService messageDialogService,IProgrammingLanguageRepository programmingLanguageRepository) : base(EventAggregator, messageDialogService)
        {
            _programmingLanguageRepository = programmingLanguageRepository;
            Title = "Programming Languages";
            ProgrammingLanguages = new ObservableCollection<ProgrammingLanguageWrapper>();
            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);

        }

        private bool OnRemoveCanExecute()
        {
            return SelectedProgrammingLanguage != null;
        }

        private async void OnRemoveExecute()
        {
            var isReferenced = await _programmingLanguageRepository.IsReferencedByFriendAsync(SelectedProgrammingLanguage.Id);
            if (isReferenced)
            {
                MessageDialogeService.ShowInfoDialog($"The language {SelectedProgrammingLanguage.Name} is already a favorite language for a friend it's cannot be deleted");
                return;
            }
            SelectedProgrammingLanguage.PropertyChanged -= Wrapper_PropertyChanged;
            _programmingLanguageRepository.Delete(SelectedProgrammingLanguage.Model);
            ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            SelectedProgrammingLanguage = null;
            HasChanged = _programmingLanguageRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddExecute()
        {
            var wrapper = new ProgrammingLanguageWrapper(new ProgrammingLanguage());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _programmingLanguageRepository.Add(wrapper.Model);
            ProgrammingLanguages.Add(wrapper);
            wrapper.Name = "";
        }

        public ObservableCollection<ProgrammingLanguageWrapper> ProgrammingLanguages { get; }
        public ICommand AddCommand { get;}
        public ICommand RemoveCommand { get; }

        public ProgrammingLanguageWrapper SelectedProgrammingLanguage { get {return _selectedProgrammingLanguage; } set {
                _selectedProgrammingLanguage = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            } }
        public async override Task LoadAsync(int id)
        {
            Id = id;
            foreach(var wrapper in ProgrammingLanguages)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }
            ProgrammingLanguages.Clear();
            var languages = await _programmingLanguageRepository.GetAllAsync();
            foreach(var model in languages)
            {
                var wrapper = new ProgrammingLanguageWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ProgrammingLanguages.Add(wrapper);

            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanged)
            {
                HasChanged = _programmingLanguageRepository.HasChanges();
            }
            if(e.PropertyName == nameof(ProgrammingLanguageWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected async override void onDeleteExecute()
        {
          
        }

        protected override bool onSaveCanExecute()
        {
            return HasChanged && ProgrammingLanguages.All(p => !p.HasErrors);
        }

        protected async override void OnSaveExecute()
        {
            await OnSaveOptimisticConcurnceyAsyc(_programmingLanguageRepository.SaveAsync, () =>
            {
                HasChanged = _programmingLanguageRepository.HasChanges();
                RaiseCollectionSavedEvent();
            });
        
        }
    }
}
