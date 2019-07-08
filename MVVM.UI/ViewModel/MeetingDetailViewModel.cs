using MVVM.Model;
using MVVM.UI.Data.Repositories;
using MVVM.UI.View.Services;
using MVVM.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.UI.ViewModel
{
  public  class MeetingDetailViewModel :DetailViewModelBase , IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private IMessageDialogService _messageDialogSerivce;
        private MeetingWrapper _meeting;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,IMessageDialogService messageDialogService, IMeetingRepository meetingRepository  ) :base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogSerivce = messageDialogService; 
        }

        public MeetingWrapper Meeting
        {
            get
            {
                return _meeting; 
            }
             private set
            {
                _meeting = value;
                OnPropertyChanged();

            }
        }

        public async override Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
            ? await _meetingRepository.GetByIdAsync(meetingId.Value) : createNewMeeting();

            InitializeMeeting(meeting);
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {

                if (!HasChanged)
                {

                    HasChanged = _meetingRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if(Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
        }

        private Meeting createNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected override void onDeleteExecute()
        {
            var result = _messageDialogSerivce.ShowOkCancelDialog($"Do you really want to delete it{Meeting.Title} ", "Warning");
            if(result == MessageDialogResult.Ok)
            {
                _meetingRepository.Delete(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool onSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanged;
        }

        protected  async override void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanged = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}
