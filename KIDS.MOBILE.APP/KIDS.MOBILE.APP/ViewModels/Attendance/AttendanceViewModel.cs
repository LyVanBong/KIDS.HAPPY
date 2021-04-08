using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Attendance
{
    public class AttendanceViewModel : BaseViewModel
    {
        private string _teacherId;
        private string _unSelectAll;
        private string[] _parents;
        private IUserService _userService;
        private string _classId;
        private IAttendanceService _attendanceService;
        private IPageDialogService _pageDialogService;
        private CountAttendanceModel _countAttendance;
        private List<AttendanceComeModel> _attendanceComes;
        private IDialogService _dialogService;
        private List<AttendanceLeaveModel> _attendanceLeave;
        private int _selectIndexTabview;
        private List<ParentModel> _parentStudent;
        private bool _isLoading;
        private string _searchCome;
        private string _searchLeave;

        public string SearchCome
        {
            get => _searchCome;
            set
            {
                if (SetProperty(ref _searchCome, value))
                    Task.Run(async () =>
                    {
                        if (string.IsNullOrWhiteSpace(SearchCome))
                        {
                            AttendanceComes = await GetAttendanceCome(DateData);
                        }
                        else
                        {
                            AttendanceComes = new List<AttendanceComeModel>(AttendanceComes.Where(x => x.Name.ToUpper().Contains(SearchCome.ToUpper())));
                        }
                    });
            }
        }

        public string SearchLeave
        {
            get => _searchLeave;
            set
            {
                if (SetProperty(ref _searchLeave, value))
                    if (string.IsNullOrWhiteSpace(SearchLeave))
                    {
                        AttendanceLeave = GetAttendanceLeave(DateData).Result;
                    }
                    else
                    {
                        AttendanceLeave = new List<AttendanceLeaveModel>(AttendanceLeave.Where(x => x.Name.ToUpper().Contains(SearchLeave.ToUpper())));
                    }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
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

        public List<ParentModel> ParentStudent
        {
            get => _parentStudent;
            set => SetProperty(ref _parentStudent, value);
        }

        public ICommand SelectPickerCommand { get; set; }
        public ICommand AttendanceLeaveCommand { get; set; }
        public ICommand SelectDateCommand { get; set; }

        public int SelectIndexTabview
        {
            get => _selectIndexTabview;
            set
            {
                if (SetProperty(ref _selectIndexTabview, value))
                {
                    Task.Run(async () =>
                    {
                        IsLoading = true;
                        if (SelectIndexTabview == 0)
                        {
                            AttendanceComes = new List<AttendanceComeModel>(await GetAttendanceCome(DateData));
                        }
                        else
                        {
                            AttendanceLeave = new List<AttendanceLeaveModel>(await GetAttendanceLeave(DateData));
                        }

                        IsLoading = false;
                    });
                }
            }
        }

        public List<AttendanceLeaveModel> AttendanceLeave
        {
            get => _attendanceLeave;
            set => SetProperty(ref _attendanceLeave, value);
        }

        public List<AttendanceComeModel> AttendanceComes
        {
            get => _attendanceComes;
            set => SetProperty(ref _attendanceComes, value);
        }

        public CountAttendanceModel CountAttendance
        {
            get => _countAttendance;
            set => SetProperty(ref _countAttendance, value);
        }

        public ICommand AttendanceComeCommand { get; private set; }

        public AttendanceViewModel(IPageDialogService pageDialogService, IAttendanceService attendanceService, IUserService userService, IDialogService dialogService)
        {
            _userService = userService;
            _attendanceService = attendanceService;
            _pageDialogService = pageDialogService;
            _dialogService = dialogService;
            AttendanceComeCommand = new Command<AttendanceComeModel>(async (come) => await AttendanceComeUpdate(come));
            AttendanceLeaveCommand = new Command<AttendanceLeaveModel>(async (leave) => await AttendanceLeaveUpdate(leave));
            SelectPickerCommand = new Command<AttendanceLeaveModel>(async (obj) => await SelectPicker(obj));
            SelectDateCommand = new Command(OpenDatePicker);
        }

        private async Task SelectPicker(AttendanceLeaveModel student)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (student != null)
                {
                    var data = await _userService.GetParentOfStudent(student.StudentID);
                    if (data != null && data.Code > 0 && data.Data.Any())
                    {
                        ParentStudent = new List<ParentModel>(data.Data);
                        var count = ParentStudent.Count;
                        if (count < 1)
                        {
                            return;
                        }

                        _parents = new string[10];
                        for (int i = 0; i < ParentStudent.Count; i++)
                        {
                            var p = ParentStudent[i];
                            _parents[i] = $"{p.Name}({p.Relationship})";
                        }

                        var action = await _pageDialogService.DisplayActionSheetAsync(AppResources._00078,
                            AppResources._00049, AppResources._00080, _parents);
                        switch (action)
                        {
                            case "Hủy bỏ":
                            case "Cancel":
                                return;

                            case "Unselect all":
                            case "Bỏ chọn":
                                student.NguoiDon = null;
                                student.ParentName = null;
                                break;

                            default:
                                var nameParent = action.Split('(');
                                student.ParentName = nameParent[0];
                                student.NguoiDon = data.Data?.Where(x => x.Name == nameParent[0]).FirstOrDefault()?.ID;
                                break;
                        }

                        await AttendanceLeaveUpdate(student);
                    }
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

        private async Task AttendanceLeaveUpdate(AttendanceLeaveModel leave)
        {
            try
            {
                IsLoading = true;
                if (leave != null)
                {
                    var update = AttendanceLeave.FirstOrDefault(x => x.ID == leave.ID);
                    if (update != null)
                    {
                        update.CoMat = leave.CoMat;
                        update.NghiCoPhep = leave.NghiCoPhep;
                        update.NghiKhongPhep = leave.NghiKhongPhep;
                        update.DiMuon = leave.DiMuon;
                        update.ArriveComment = leave.ArriveComment;
                    }
                    var data = await _attendanceService.AttendanceUpdateAfternoon(leave.ID, leave.Leave,
                        leave.LeaveLate, leave.LeaveComment, leave.NguoiDon, _teacherId);
                    if (data.Code > 0)
                    {

                    }
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

        private async Task AttendanceComeUpdate(AttendanceComeModel come)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (come != null)
                {
                    var update = AttendanceComes.FirstOrDefault(x => x.ID == come.ID);
                    if (update != null)
                    {
                        update.CoMat = come.CoMat;
                        update.NghiCoPhep = come.NghiCoPhep;
                        update.NghiKhongPhep = come.NghiKhongPhep;
                        update.DiMuon = come.DiMuon;
                        update.ArriveComment = come.ArriveComment;
                    }
                    var data = await _attendanceService.AttendanceUpdateMorning(come.ID, come.CoMat, come.NghiCoPhep,
                        come.NghiKhongPhep, come.DiMuon, come.ArriveComment, _teacherId);
                    if (data.Code > 0)
                    {
                        await AttendanceCount();
                    }
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            DateData = DateTime.Now;
            ChoosedDate = DateData.ToString("dd/MM/yyyy");
            await InitializationAttendance();
        }

        private void OpenDatePicker()
        {
            var para = new DialogParameters();
            para.Add(nameof(ChoosedDate), ChoosedDate);
            _dialogService.ShowDialog(nameof(DatePickerDialog), para, goBackAttendance);
        }

        private async void goBackAttendance(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.ChoosedDate))
            {
                var date = obj.Parameters.GetValue<DateTime>(AppConstants.ChoosedDate);
                DateData = date;
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
            }
            await InitializationAttendance();
        }

        private async Task InitializationAttendance()
        {
            try
            {
                IsLoading = true;
                _classId = AppConstants.User.ClassID;
                _teacherId = AppConstants.User.NguoiSuDung;
                AttendanceComes = await GetAttendanceCome(DateTime.Now);
                await AttendanceCount();
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

        private async Task<List<AttendanceLeaveModel>> GetAttendanceLeave(DateTime date)
        {
            try
            {
                var data = await _attendanceService.AttendanceLeave(_classId,
                    date.ToString("yyyy/MM/dd"));
                if (data.Code > 0)
                {
                    return new List<AttendanceLeaveModel>(data.Data);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        private async Task<List<AttendanceComeModel>> GetAttendanceCome(DateTime date)
        {
            try
            {
                var comes = await _attendanceService.AttendanceCome(_classId,
                    date.ToString("yyyy/MM/dd"));
                if (comes.Code > 0)
                {
                    return comes.Data.ToList();
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }

        private async Task AttendanceCount(string fromDate = null, string toDate = null)
        {
            try
            {
                if (fromDate == null && toDate == null)
                    fromDate = toDate = DateTime.Now.ToString("yyyy/MM/dd");
                var data = await _attendanceService.CountAttendance(_classId, fromDate, toDate);
                if (data?.Code > 0)
                {
                    CountAttendance = data.Data.FirstOrDefault();
                }
                else
                {
                    CountAttendance = new CountAttendanceModel
                    {
                        CoMat = 0,
                        DiMuon = 0,
                        NghiCoPhep = 0,
                        NghiKhongPhep = 0,
                        STT = 0
                    };
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}