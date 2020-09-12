using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Models.Learn;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.Learn;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Learn
{
    public class LearnViewModel : BaseViewModel
    {
        private bool _isHasData;
        private string _classId;
        private ILearnService _learnService;
        private List<LearnModel> _learnMorningData;
        private IDialogService _dialogService;
        private string _time;
        private List<LearnModel> _learnAfternoonData;
        private bool _isLoading;
        private List<AttendanceLeaveModel> _studentData;
        private IAttendanceService _attendanceService;
        private string _searchMorning;
        private List<AttendanceLeaveModel> _cache;
        private string _searchAfternoon;

        public bool IsHasData
        {
            get => _isHasData;
            set => SetProperty(ref _isHasData, value);
        }

        public string SearchAfternoon
        {
            get => _searchAfternoon;
            set
            {
                if (SetProperty(ref _searchAfternoon, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchAfternoon))
                    {
                        StudentData = _cache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchAfternoon.ToUpper())));
                    }
                }
            }
        }

        public string SearchMorning
        {
            get => _searchMorning;
            set
            {
                if (SetProperty(ref _searchMorning, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchMorning))
                    {
                        StudentData = _cache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchMorning.ToUpper())));
                    }
                }
            }
        }

        private DateTime _dateData;

        public DateTime DateData
        {
            get => _dateData;
            set => SetProperty(ref _dateData, value);
        }

        private string _choosedDate;

        public string ChoosedDate
        {
            get => _choosedDate;
            set => SetProperty(ref _choosedDate, value);
        }

        public List<AttendanceLeaveModel> StudentData
        {
            get => _studentData;
            set => SetProperty(ref _studentData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<LearnModel> LearnAfternoonData
        {
            get => _learnAfternoonData;
            set => SetProperty(ref _learnAfternoonData, value);
        }

        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public List<LearnModel> LearnMorningData
        {
            get => _learnMorningData;
            set => SetProperty(ref _learnMorningData, value);
        }

        public ICommand UpdateMorningCommand { get; set; }
        public ICommand UpdateAfternoonCommand { get; private set; }
        public ICommand SelectDateCommand { get; set; }

        public LearnViewModel(ILearnService learnService, IAttendanceService attendanceService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _learnService = learnService;
            _attendanceService = attendanceService;
            UpdateMorningCommand = new Command<AttendanceLeaveModel>(UpdateMorning);
            UpdateAfternoonCommand = new Command<AttendanceLeaveModel>(UpdateAfternoon);
            SelectDateCommand = new Command(OpenDatePicker);
        }

        private async void UpdateAfternoon(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var updateMorning =
                    await _learnService.UpdatePlanStudyAfternoon(obj.ID, obj.UserCreate, obj.StudyCommentPM);
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

        private async void UpdateMorning(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var updateMorning =
                    await _learnService.UpdatePlanStudyMorning(obj.ID, obj.UserCreate, obj.StudyCommentAM);
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            DateData = DateTime.Now;
            ChoosedDate = DateData.ToString("dd/MM/yyyy");
            await InitializationLearn();
        }

        private void OpenDatePicker()
        {
            var para = new DialogParameters();
            para.Add(nameof(ChoosedDate), ChoosedDate);
            _dialogService.ShowDialog(nameof(DatePickerDialog), para, goBackLearn);
        }

        private async void goBackLearn(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.ChoosedDate))
            {
                var date = obj.Parameters.GetValue<DateTime>(AppConstants.ChoosedDate);
                DateData = date;
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
            }
            await InitializationLearn();
        }

        private async Task InitializationLearn()
        {
            try
            {
                IsLoading = true;
                _classId = AppConstants.User.ClassID;
                var date = DateData.ToString("yyyy/MM/dd");
                var lsAttendance = await _attendanceService.AttendanceLeave(_classId, date);
                if (lsAttendance.Code > 0)
                {
                    _cache = StudentData = new List<AttendanceLeaveModel>(lsAttendance.Data);
                }
                //Time = DateData.ToString("F");
                Time = ChoosedDate;
                var data = await _learnService.PlanStudyMorning(date, _classId);
                if (data.Code > 0)
                {
                    LearnMorningData = new List<LearnModel>(data.Data);
                    IsHasData = true;
                }
                else
                {
                    LearnMorningData = new List<LearnModel>();
                    IsHasData = false;
                }

                var afternoon = await _learnService.PlanStudyAfternoon(date,
                    _classId);
                if (afternoon.Code > 0)
                {
                    LearnAfternoonData = new List<LearnModel>(afternoon.Data);
                    IsHasData = true;
                }
                else
                {
                    LearnAfternoonData = new List<LearnModel>();
                    IsHasData = false;
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