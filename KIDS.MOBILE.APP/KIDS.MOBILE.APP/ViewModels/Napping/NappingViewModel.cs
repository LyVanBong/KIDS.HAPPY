using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Controls.Dialogs.TimePicker;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.Napping;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Napping
{
    public class NappingViewModel : BaseViewModel
    {
        private string _searchNapping;
        private DateTime _dateData;
        private string _choosedDate;
        private string _nguoiDung;
        private List<AttendanceLeaveModel> _studentData;
        private List<AttendanceLeaveModel> _cache;
        private string _startNapping;
        private string _endNapping;
        private IDialogService _dialogService;
        private IAttendanceService _attendanceService;
        private bool _isLoading;
        private INappingService _navigationService;
        public string SearchNapping
        {
            get => _searchNapping;
            set
            {
                if (SetProperty(ref _searchNapping, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchNapping))
                    {
                        StudentData = _cache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchNapping.ToUpper())));
                    }
                }
            }
        }
        public DateTime DateData
        {
            get => _dateData;
            set => SetProperty(ref _dateData, value);
        }
        public string ChoosedDate
        {
            get => _choosedDate;
            set => SetProperty(ref _choosedDate, value);
        }
       
        public ICommand UpdateStartTimeSleepCommand { get; private set; }
        public ICommand UpdateEndTimeSleepCommand { get; private set; }
        public ICommand UpdateNappingCommand { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand UpdateCommand { get; set; }
        public ICommand EndNappingCommand { get; set; }
        public ICommand StartNappingCommand { get; set; }
        public ICommand SelectDateCommand { get; set; }


        public string EndNapping
        {
            get => string.IsNullOrWhiteSpace(_endNapping) ? "14:45" : _endNapping;
            set => SetProperty(ref _endNapping, value);
        }

        public string StartNapping
        {
            get => string.IsNullOrWhiteSpace(_startNapping) ? "12:00" : _startNapping;
            set => SetProperty(ref _startNapping, value);
        }

        public List<AttendanceLeaveModel> StudentData
        {
            get => _studentData;
            set => SetProperty(ref _studentData, value);
        }

        public NappingViewModel(IDialogService dialogService, IAttendanceService attendanceService, INappingService nappingService)
        {
            _navigationService = nappingService;
            _attendanceService = attendanceService;
            _dialogService = dialogService;
            StartNappingCommand = new Command(StartNappingOpen);
            EndNappingCommand = new Command(EndNappingOpen);
            UpdateCommand = new Command(Update);
            UpdateNappingCommand = new Command<AttendanceLeaveModel>(UpdateNapping);
            UpdateStartTimeSleepCommand = new Command<AttendanceLeaveModel>(UpdateStartTimeSleep);
            UpdateEndTimeSleepCommand = new Command<AttendanceLeaveModel>(UpdateEndTimeSleep);
            SelectDateCommand = new Command(OpenDatePicker);
        }
        private void OpenDatePicker()
        {
            var para = new DialogParameters();
            para.Add(nameof(ChoosedDate), ChoosedDate);
            _dialogService.ShowDialog(nameof(DatePickerDialog), para, goBackNapping);
        }
        private async void goBackNapping(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.ChoosedDate))
            {
                var date = obj.Parameters.GetValue<DateTime>(AppConstants.ChoosedDate);
                DateData = date;
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
            }
            await InitialNapping();
        }
        private void UpdateEndTimeSleep(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var para = new DialogParameters();
                para.Add("Time", obj.SleepTo);
                _dialogService.ShowDialog(nameof(TimePickerDialog), para, result =>
                {
                    var time = result.Parameters.GetValue<TimeSpan>(AppConstants.TimeSelect);
                    obj.SleepTo = $"{time.Hours}:{time.Minutes}";
                    UpdateNapping(obj);
                });
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

        private void UpdateStartTimeSleep(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var para = new DialogParameters();
                para.Add("Time", obj.SleepFrom);
                _dialogService.ShowDialog(nameof(TimePickerDialog), para, result =>
                {
                    var time = result.Parameters.GetValue<TimeSpan>(AppConstants.TimeSelect);
                    obj.SleepFrom = $"{time.Hours}:{time.Minutes}";
                    UpdateNapping(obj);
                });
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

        private async void UpdateNapping(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var update = await _navigationService.UpdateNapping(obj.ID, _nguoiDung, obj.SleepFrom, obj.SleepTo,
                    obj.SleepComment);
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

        private void Update()
        {
            IsLoading = true;
            StudentData.ForEach(x =>
            {
                x.SleepFrom = StartNapping;
                x.SleepTo = EndNapping;
            });
            IsLoading = false;
        }

        private void EndNappingOpen()
        {
            var para = new DialogParameters();
            para.Add(nameof(EndNapping), EndNapping);
            _dialogService.ShowDialog(nameof(TimePickerDialog), para, GoBackEndNapping);
        }

        private void GoBackEndNapping(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.TimeSelect))
            {
                var time = obj.Parameters.GetValue<TimeSpan>(AppConstants.TimeSelect);
                EndNapping = $"{time.Hours}:{time.Minutes}";
            }
        }

        private void StartNappingOpen()
        {
            var para = new DialogParameters();
            para.Add(nameof(StartNapping), StartNapping);
            _dialogService.ShowDialog(nameof(TimePickerDialog), para, GoBackStartNapping);
        }

        private void GoBackStartNapping(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.TimeSelect))
            {
                var time = obj.Parameters.GetValue<TimeSpan>(AppConstants.TimeSelect);
                StartNapping = $"{time.Hours}:{time.Minutes}";
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            DateData = DateTime.Now;
            ChoosedDate = DateData.ToString("dd/MM/yyyy");
            try
            {
                IsLoading = true;
                _nguoiDung = AppConstants.User.NguoiSuDung;
                if (parameters != null)
                {
                    if (parameters.ContainsKey("EndTime"))
                    {
                        var endTime = parameters["EndTime"] as string;
                        var time = parameters["Time"] as string;
                        if (endTime == "0")
                        {
                            StartNapping = time;
                        }
                        else
                        {
                            EndNapping = time;
                        }
                    }
                }

                await InitialNapping();
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
        private async Task InitialNapping()
        {
            try
            {
                IsLoading = true;
                var data = await _attendanceService.AttendanceLeave(AppConstants.User.ClassID,
                       DateData.ToString("yyyy/MM/dd"));
                if (data.Code > 0)
                {
                    _cache = StudentData = new List<AttendanceLeaveModel>(data.Data);
                    StartNapping = DateTime.Parse(StudentData[0].SleepFrom).ToString("HH:mm");
                    EndNapping = DateTime.Parse(StudentData[0].SleepTo).ToString("HH:mm");
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
    }
}