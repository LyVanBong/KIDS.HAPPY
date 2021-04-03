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
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.views.Learn;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
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
        private IPageDialogService _pageDialogService;

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
        private bool _isOpenFastFeature;
        private string _thoiKhoaBieu = "Thời khóa biểu";
        private string _thoiKhoaBieuChieu = "Thời khóa biểu";

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
        /// <summary>
        /// Cap nhat nhanh
        /// </summary>
        public ICommand QuickCommentCommand { get; private set; }

        public bool IsOpenFastFeature
        {
            get => _isOpenFastFeature;
            set => SetProperty(ref _isOpenFastFeature, value);
        }

        public ICommand FastFeatureCommand { get; private set; }

        public string ThoiKhoaBieu
        {
            get => _thoiKhoaBieu;
            set => SetProperty(ref _thoiKhoaBieu, value);
        }

        public string ThoiKhoaBieuChieu
        {
            get => _thoiKhoaBieuChieu;
            set => SetProperty(ref _thoiKhoaBieuChieu, value);
        }

        public LearnViewModel(ILearnService learnService, IAttendanceService attendanceService, IDialogService dialogService, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _dialogService = dialogService;
            _learnService = learnService;
            _attendanceService = attendanceService;
            SelectDateCommand = new Command(OpenDatePicker);
            FastFeatureCommand = new AsyncCommand<string>(async (key) => await FastFeature(key));
        }

        private async Task FastFeature(string key)
        {
            if (IsLoading) return;
            IsLoading = true;
            switch (key)
            {
                case "0":
                    IsOpenFastFeature = !IsOpenFastFeature;
                    break;
                case "1":
                case "3":
                    await QuickComment(key);
                    break;
                case "2":
                case "4":
                    await StadyComment(key);
                    break;
                default:
                    break;
            }

            IsLoading = false;
        }

        private async Task StadyComment(string key)
        {
            try
            {
                if (key == "2")
                {
                    var cmt = StudentData.Where(x => x.StudyCommentAM == null)?.ToList();
                    if (cmt.Any())
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Vẫn còn có con chưa được nhận xét", "OK");
                    }
                    else
                    {
                        var count = 0;
                        foreach (var item in StudentData)
                        {
                            count += await UpdateMorning(item);
                        }
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Đã nhận xét " + count + " con thanh công", "OK");
                        IsOpenFastFeature = false;
                    }
                }
                else
                {
                    var cmt = StudentData.Where(x => x.StudyCommentPM == null)?.ToList();
                    if (cmt.Any())
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Vẫn còn có con chưa được nhận xét", "OK");
                    }
                    else
                    {
                        var count = 0;
                        foreach (var item in StudentData)
                        {
                            count += await UpdateAfternoon(item);
                        }
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Đã nhận xét " + count + " con thanh công", "OK");
                        IsOpenFastFeature = false;
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task QuickComment(string key)
        {
            try
            {
                _dialogService.ShowDialog("QuickCommentDialog", result =>
                {
                    var para = result.Parameters.GetValue<string>("CommentContent");
                    if (para != null)
                    {
                        if (key == "1")
                        {
                            foreach (var student in StudentData)
                            {
                                if (string.IsNullOrWhiteSpace(student.StudyCommentAM))
                                {
                                    student.StudyCommentAM = para;
                                }
                            }
                        }
                        else
                        {
                            foreach (var student in StudentData)
                            {
                                if (string.IsNullOrWhiteSpace(student.StudyCommentPM))
                                {
                                    student.StudyCommentPM = para;
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task<int> UpdateAfternoon(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var updateMorning =
                    await _learnService.UpdatePlanStudyAfternoon(obj.ID, obj.UserCreate, obj.StudyCommentPM);
                return updateMorning.Data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }

            return 0;
        }

        private async Task<int> UpdateMorning(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var updateMorning =
                    await _learnService.UpdatePlanStudyMorning(obj.ID, obj.UserCreate, obj.StudyCommentAM);
                return updateMorning.Data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }

            return 0;
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
                    if (LearnMorningData != null && LearnMorningData.Any())
                    {
                        ThoiKhoaBieu = LearnMorningData[0].Title != null ? LearnMorningData[0].Title : ThoiKhoaBieu;
                    }
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
                    if (LearnAfternoonData != null && LearnAfternoonData.Any())
                    {
                        ThoiKhoaBieuChieu = LearnAfternoonData[0].Title != null ? LearnAfternoonData[0].Title : ThoiKhoaBieuChieu;
                    }
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