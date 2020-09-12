using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Models.Assessment;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Services.Assessment;
using KIDS.MOBILE.APP.Services.Attendance;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Assessment
{
    public class AssessmentViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IAssessmentService _assessmentService;
        private IAttendanceService _attendanceService;
        private List<AttendanceLeaveModel> _attendanceLeaves;
        private List<AttendanceLeaveModel> _cache;
        private IDialogService _dialogService;
        private int _selectIndexTabview;
        private bool _isLoading;
        private DateTime _dateData;
        private string _searchWeek;
        private string _searchDay;

        public string SearchWeek
        {
            get => _searchWeek;
            set
            {
                if (SetProperty(ref _searchWeek, value))
                    if (string.IsNullOrWhiteSpace(SearchWeek))
                    {
                        AttendanceLeave = _cache;
                    }
                    else
                    {
                        AttendanceLeave = new List<AttendanceLeaveModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchWeek.ToUpper())));
                    }
            }
        }

        public string SearchDay
        {
            get => _searchDay;
            set
            {
                if (SetProperty(ref _searchDay, value))
                    if (string.IsNullOrWhiteSpace(SearchDay))
                    {
                        AttendanceLeave = _cache;
                    }
                    else
                    {
                        AttendanceLeave = new List<AttendanceLeaveModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchDay.ToUpper())));
                    }
            }
        }

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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<AttendanceLeaveModel> AttendanceLeave
        {
            get => _attendanceLeaves;
            set => SetProperty(ref _attendanceLeaves, value);
        }

        //public ICommand SelectPickerCommand { get; set; }
        public ICommand AttendanceLeaveCommand { get; set; }

        public ICommand ReloadCommand { get; set; }
        public ICommand SelectDateCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public int SelectIndexTabview
        {
            get => _selectIndexTabview;
            set => SetProperty(ref _selectIndexTabview, value);
        }

        public AssessmentViewModel(IAttendanceService attendanceService, IAssessmentService assessmentService, INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _attendanceService = attendanceService;
            _assessmentService = assessmentService;
            _dialogService = dialogService;
            //SelectPickerCommand = new Command<AttendanceLeaveModel>(async (obj) => await SelectPicker(obj));
            AttendanceLeaveCommand = new Command<AttendanceLeaveModel>(async (leave) => await AttendanceLeaveUpdate(leave));
            ReloadCommand = new Command(async () => await InitializationAttendance());
            SelectDateCommand = new Command(OpenDatePicker);
            SearchCommand = new Command(Search);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            DateData = DateTime.Now;
            ChoosedDate = DateData.ToString("dd/MM/yyyy");
            await InitializationAttendance();
        }

        private void Search()
        {
        }

        private async Task InitializationAttendance()
        {
            try
            {
                IsLoading = true;
                await GetAttendanceLeave();
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

        private async Task AttendanceLeaveUpdate(AttendanceLeaveModel leave)
        {
            try
            {
                IsLoading = true;
                var student = new AssessmentModel();
                if (leave != null)
                {
                    student.ID = leave.ID;
                    student.UserCreate = AppConstants.User.NguoiSuDung;
                    if (SelectIndexTabview == 0)
                    {
                        student.PhieuBeNgoan = leave.PhieuBeNgoan;
                        student.Comment = leave.DayComment;
                    }
                    else
                    {
                        student.PhieuBeNgoan = leave.WeekPhieuBeNgoan;
                        student.Comment = leave.WeekComment;
                    }
                }
                if (SelectIndexTabview == 0)
                    await DailyAssessmentUpdate(student);
                else
                    await WeeklyAssessmentUpdate(student);
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

        private async Task GetAttendanceLeave()
        {
            try
            {
                var data = await _attendanceService.AttendanceLeave(AppConstants.User.ClassID,
                    DateData.ToString("yyyy/MM/dd"));
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
                if (data.Code > 0)
                {
                    _cache = AttendanceLeave = new List<AttendanceLeaveModel>(data.Data);
                    foreach (var item in AttendanceLeave)
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        //private async Task SelectPicker(AttendanceLeaveModel leave)
        //{
        //    try
        //    {
        //        IsLoading = true;
        //        if (leave != null)
        //        {
        //            var student = new AssessmentModel();
        //            if (leave != null)
        //            {
        //                student.ID = leave.ID;
        //                student.UserCreate = AppConstants.User.NguoiSuDung;
        //                student.PhieuBeNgoan = leave.PhieuBeNgoan;
        //                if (SelectIndexTabview == 0)
        //                    student.Comment = leave.DayComment;
        //                else
        //                    student.Comment = leave.WeekComment;
        //            }
        //            if (SelectIndexTabview == 0)
        //                await DailyAssessmentUpdate(student);
        //            else
        //                await WeeklyAssessmentUpdate(student);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Crashes.TrackError(e);
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //}
        private void OpenDatePicker()
        {
            var para = new DialogParameters();
            para.Add(nameof(ChoosedDate), ChoosedDate);
            _dialogService.ShowDialog(nameof(DatePickerDialog), para, goBackAssessment);
        }

        private void goBackAssessment(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.ChoosedDate))
            {
                var date = obj.Parameters.GetValue<DateTime>(AppConstants.ChoosedDate);
                DateData = date;
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
            }
        }

        private async Task DailyAssessmentUpdate(AssessmentModel student)
        {
            try
            {
                IsLoading = true;
                if (student.ID != null && student.UserCreate != null)
                {
                    var data = await _assessmentService.DailyAssessmentAdd(student);
                    if (data.Code > 0)
                    {
                    }
                }
                await InitializationAttendance();
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

        private async Task WeeklyAssessmentUpdate(AssessmentModel student)
        {
            try
            {
                IsLoading = true;
                if (student.ID != null && student.UserCreate != null)
                {
                    var data = await _assessmentService.WeaklyAssessmentAdd(student);
                    if (data.Code > 0)
                    {
                    }
                }
                await InitializationAttendance();
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

        private void GoBack()
        {
            _navigationService.GoBackAsync();
        }
    }
}