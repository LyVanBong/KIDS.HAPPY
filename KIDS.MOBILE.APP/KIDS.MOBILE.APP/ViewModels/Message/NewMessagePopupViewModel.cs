using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.Message;
using Microsoft.AppCenter.Crashes;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Message
{
    public class NewMessagePopupViewModel : BindableBase, IDialogAware
    {
        private IAttendanceService _attendanceService;
        private ObservableCollection<AttendanceLeaveModel> _attendanceLeaveData;
        private bool _isLoading;
        private string _searchPeopleName;
        private List<AttendanceLeaveModel> _studentData;
        private bool _isSendMessage;
        private List<AttendanceLeaveModel> _selectData;
        private string _namePeopleSent;
        private string _bodyMessage;
        private bool _isActiveButtonSent;
        private IMessageService _messageService;
        private string[] _parameterSentMessage;
        public ICommand SendMessageCommand { get; private set; }

        public bool IsActiveButtonSent
        {
            get => _isActiveButtonSent;
            set => SetProperty(ref _isActiveButtonSent, value);
        }

        public string BodyMessage
        {
            get => _bodyMessage;
            set => SetProperty(ref _bodyMessage, value, CheckSentMessage);
        }

        public ICommand ReSelectPeopleSentCommand { get; private set; }

        public string NamePeopleSent
        {
            get => _namePeopleSent;
            set => SetProperty(ref _namePeopleSent, value);
        }

        public ICommand ChoosePeopleCommand { get; private set; }

        public bool IsSendMessage
        {
            get => _isSendMessage;
            set => SetProperty(ref _isSendMessage, value);
        }

        public string SearchPeopleName
        {
            get => _searchPeopleName;
            set => SetProperty(ref _searchPeopleName, value, async () => await SearchPeople(SearchPeopleName));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<AttendanceLeaveModel> AttendanceLeaveData
        {
            get => _attendanceLeaveData;
            set => SetProperty(ref _attendanceLeaveData, value);
        }

        public ICommand CancelNewMessageCommand { get; private set; }

        public NewMessagePopupViewModel(IAttendanceService attendanceService, IMessageService messageService)
        {
            _attendanceService = attendanceService;
            _messageService = messageService;
            CancelNewMessageCommand = new Command(CancelNewMessage);
            ChoosePeopleCommand = new Command(ChoosePeople);
            ReSelectPeopleSentCommand = new Command(() => IsSendMessage = false);
            SendMessageCommand = new Command(async () => await SendMessage());
        }

        private async Task SendMessage()
        {
            try
            {
                IsLoading = true;
                for (int i = 0; i < _selectData.Count; i++)
                {
                    var data = await _messageService.CreateSentandComment(_parameterSentMessage[0], _parameterSentMessage[1], null,
                          BodyMessage, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), _selectData[i].StudentID, "1");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
                CancelNewMessage();
            }
        }

        private void CheckSentMessage()
        {
            if (string.IsNullOrWhiteSpace(BodyMessage))
            {
                IsActiveButtonSent = false;
            }
            else
            {
                IsActiveButtonSent = true;
            }
        }

        private void ChoosePeople()
        {
            try
            {
                IsLoading = true;
                var dataCheck = AttendanceLeaveData.Where(x => x.IsSelected == true);
                if (dataCheck.Any())
                {
                    IsSendMessage = true;
                    _selectData = dataCheck.ToList();
                    NamePeopleSent = null;
                    _selectData.ForEach(x =>
                    {
                        NamePeopleSent += x.Name + ", ";
                    });
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchPeople(string search)
        {
            if (search == null)
            {
                AttendanceLeaveData = new ObservableCollection<AttendanceLeaveModel>(_studentData);
            }
            else
            {
                AttendanceLeaveData = new ObservableCollection<AttendanceLeaveModel>(_studentData.Where(x => x.Name.ToUpper().Contains(search.ToUpper())));
            }
        }

        private void CancelNewMessage()
        {
            if (RequestClose != null)
            {
                RequestClose(new DialogParameters());
            }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                IsLoading = true;
                _parameterSentMessage = new[] { AppConstants.User.ClassID, AppConstants.User.NguoiSuDung };
                var data = await _attendanceService.AttendanceLeave(_parameterSentMessage[0],
                    DateTime.Now.ToString("yyyy/MM/dd"));
                if (data != null && data.Code > 0)
                {
                    AttendanceLeaveData = new ObservableCollection<AttendanceLeaveModel>(data.Data);
                    _studentData = new List<AttendanceLeaveModel>(data.Data);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}