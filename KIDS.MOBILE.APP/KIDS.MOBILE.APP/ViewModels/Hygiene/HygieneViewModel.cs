using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.Hygiene;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Hygiene
{
    public class HygieneViewModel : BaseViewModel
    {

        private bool _isLoading;
        private List<AttendanceLeaveModel> _studentData;
        private List<AttendanceLeaveModel> _cache;
        private IAttendanceService _attendanceService;
        private IDialogService _dialogService;
        private IHygieneService _hygieneService;
        private DateTime _dateData;
        private string _searchHygiene;
        public string SearchHygiene
        {
            get => _searchHygiene;
            set
            {
                if (SetProperty(ref _searchHygiene, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchHygiene))
                        StudentData = _cache;
                    else
                        StudentData = new List<AttendanceLeaveModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchHygiene.ToUpper())));
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
        public ICommand PlusCommand { get; private set; }
        public ICommand MinusCommand { get; private set; }
        public ICommand UpdateHygieneCommand { get; private set; }
        public ICommand SelectDateCommand { get; set; }

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

        public HygieneViewModel(IAttendanceService attendanceService, IHygieneService hygieneService,IDialogService dialogService)
        {
            _dialogService = dialogService;
            _hygieneService = hygieneService;
            _attendanceService = attendanceService;
            UpdateHygieneCommand = new Command<AttendanceLeaveModel>(UpdateHygiene);
            MinusCommand = new Command<AttendanceLeaveModel>(MinusHygiene);
            PlusCommand = new Command<AttendanceLeaveModel>(PlusHygiene);
            SelectDateCommand = new Command(OpenDatePicker);
        }
        private void OpenDatePicker()
        {
            var para = new DialogParameters();
            para.Add(nameof(ChoosedDate), ChoosedDate);
            _dialogService.ShowDialog(nameof(DatePickerDialog), para, goBackHygiene);
        }
        private async void goBackHygiene(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.ChoosedDate))
            {
                var date = obj.Parameters.GetValue<DateTime>(AppConstants.ChoosedDate);
                DateData = date;
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
            }
           await InitializeHygiene();
        }
        private void PlusHygiene(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                obj.Hygiene = (int.Parse(obj.Hygiene) + 1) + "";
                UpdateHygiene(obj);
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


        private void MinusHygiene(AttendanceLeaveModel obj)
        {
            if (obj.Hygiene == "0")
                return;
            try
            {
                IsLoading = true;
                obj.Hygiene = (int.Parse(obj.Hygiene) - 1) + "";
                UpdateHygiene(obj);
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

        private async void UpdateHygiene(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var data = await _hygieneService.UpdateHygiene(obj.ID, obj.UserCreate, obj.Hygiene, obj.HygieneComment);
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
            await InitializeHygiene();
        }

        private async Task InitializeHygiene()
        {
            try
            {
                IsLoading = true;
                var data = await _attendanceService.AttendanceLeave(AppConstants.User.ClassID,
                    DateData.ToString("yyyy/MM/dd"));
                if (data.Code > 0)
                {
                    _cache = StudentData = new List<AttendanceLeaveModel>(data.Data);
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