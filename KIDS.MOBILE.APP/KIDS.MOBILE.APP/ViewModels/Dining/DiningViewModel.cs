using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Models.Dining;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.Dining;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Dining
{
    public class DiningViewModel : BaseViewModel
    {
        private bool _isHaveDish;
        private string _searchSnack;
        private string _searchLunch;
        private string _searchDinner;
        private string _searchBreak;
        private DateTime _dateData;
        private string _choosedDate;
        private string _nguoiDung;
        private IDialogService _dialogService;
        private IAttendanceService _attendanceService;
        private int _selectedIndex;
        private List<AttendanceLeaveModel> _studentData;
        private List<AttendanceLeaveModel> _studentCache;
        private bool _isLoading;
        private List<DishModel> _dishData;
        private IDiningService _diningService;
        private List<MealListModel> _mealList;
        public ICommand UpdateBreakFastCommand { get; set; }
        public ICommand UpdateDinnerCommand { get; set; }
        public ICommand UpdateLunchCommand { get; set; }
        public ICommand UpdateSnackCommand { get; set; }
        public ICommand SelectDateCommand { get; set; }
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
        public List<MealListModel> MealList
        {
            get => _mealList;
            set => SetProperty(ref _mealList, value);
        }

        public List<DishModel> DishData
        {
            get => _dishData;
            set => SetProperty(ref _dishData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<AttendanceLeaveModel> StudentData
        {
            get => _studentData;
            set => SetProperty(ref _studentData, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value, LoadDataSelect);
        }
        public bool IsHaveDish 
        {
            get => _isHaveDish;
            set => SetProperty(ref _isHaveDish, value);
        }
        public string SearchSnack 
        { 
            get => _searchSnack;
            set
            {
                if (SetProperty(ref _searchSnack, value))
                    if (string.IsNullOrWhiteSpace(SearchSnack))
                    {
                        StudentData = _studentCache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_studentCache.Where(x => x.Name.ToUpper().Contains(SearchSnack.ToUpper())));
                    }
            }
        }
        public string SearchLunch 
        { 
            get => _searchLunch;
            set
            {
                if (SetProperty(ref _searchLunch, value))
                    if (string.IsNullOrWhiteSpace(SearchLunch))
                    {
                        StudentData = _studentCache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_studentCache.Where(x => x.Name.ToUpper().Contains(SearchLunch.ToUpper())));
                    }
            }
        }
        public string SearchDinner 
        { 
            get => _searchDinner;
            set
            {
                if (SetProperty(ref _searchDinner, value))
                    if (string.IsNullOrWhiteSpace(SearchDinner))
                    {
                        StudentData = _studentCache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_studentCache.Where(x => x.Name.ToUpper().Contains(SearchDinner.ToUpper())));
                    }
            }
        }
        public string SearchBreak 
        { 
            get => _searchBreak;
            set
            {
                if (SetProperty(ref _searchBreak, value))
                    if (string.IsNullOrWhiteSpace(SearchBreak))
                    {
                        StudentData = _studentCache;
                    }
                    else
                    {
                        StudentData = new List<AttendanceLeaveModel>(_studentCache.Where(x => x.Name.ToUpper().Contains(SearchBreak.ToUpper())));
                    }
            }
        }

        public DiningViewModel(IAttendanceService attendanceService, IDiningService diningService,IDialogService dialogService)
        {
            _dialogService = dialogService;
            _diningService = diningService;
            _attendanceService = attendanceService;
            UpdateBreakFastCommand = new Command<AttendanceLeaveModel>(UpdateBreakFast);
            UpdateDinnerCommand = new Command<AttendanceLeaveModel>(UpdateDinner);
            UpdateLunchCommand = new Command<AttendanceLeaveModel>(UpdateLunch);
            UpdateSnackCommand = new Command<AttendanceLeaveModel>(UpdateSnack);
            SelectDateCommand = new Command(OpenDatePicker);
        }
        private void OpenDatePicker()
        {
            var para = new DialogParameters();
            para.Add(nameof(ChoosedDate), ChoosedDate);
            _dialogService.ShowDialog(nameof(DatePickerDialog), para, goBackDining);
        }
        private async void goBackDining(IDialogResult obj)
        {
            if (obj.Parameters.ContainsKey(AppConstants.ChoosedDate))
            {
                var date = obj.Parameters.GetValue<DateTime>(AppConstants.ChoosedDate);
                DateData = date;
                ChoosedDate = DateData.ToString("dd/MM/yyyy");
            }
            await InitializeDining();
        }
        private async void UpdateSnack(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var update =
                    await _diningService.UpdateDining(obj.ID, _nguoiDung, "", "", "", obj.MealComment3, "", "");
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

        private async void UpdateLunch(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var update =
                    await _diningService.UpdateDining(obj.ID, _nguoiDung, "", obj.MealComment1, "", "", "", "");
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

        private async void UpdateDinner(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var update =
                    await _diningService.UpdateDining(obj.ID, _nguoiDung, "", "", obj.MealComment2, "", "", "");
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

        private async void UpdateBreakFast(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var update =
                    await _diningService.UpdateDining(obj.ID, _nguoiDung, obj.MealComment0, "", "", "", "", "");
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
            await InitializeDining();
        }

        private async void LoadDataSelect()
        {
            try
            {
                IsLoading = true;
                if (MealList != null && MealList.Any())
                {
                    var tmp = MealList[SelectedIndex];
                    var tmpDish = await _diningService.GetListOfDishes(tmp.ID, AppConstants.User.GradeID, DateData.ToString("yyyy/MM/dd"));
                    if (tmpDish.Code > 0)
                    {
                        var monAnData = new List<ListOfDishesModel>(tmpDish.Data);
                        var monAn = monAnData[0].MonAn;
                        if (monAn != null)
                        {
                            DishData = new List<DishModel>();
                            var lsMonAn = monAn.Split(',');
                            for (int i = 0; i < lsMonAn.Length; i++)
                            {
                                var chiTietMonAn = await _diningService.GetDishDetail(lsMonAn[i]);
                                if (chiTietMonAn.Code > 0)
                                {
                                    var tmpMonAn = chiTietMonAn.Data.ToList();
                                    DishData.Add(tmpMonAn[0]);
                                }
                            }
                        }
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

        private async Task InitializeDining()
        {
            try
            {
                IsLoading = true;
                _nguoiDung = AppConstants.User.NguoiSuDung;
                var data = await _attendanceService.AttendanceLeave(AppConstants.User.ClassID,
                    DateData.ToString("yyyy/MM/dd"));
                if (data.Code > 0)
                {
                    _studentCache = StudentData = new List<AttendanceLeaveModel>(data.Data);
                }

                var tmpMeal = await _diningService.GetMealList();
                if (tmpMeal.Code > 0)
                {
                    MealList = new List<MealListModel>(tmpMeal.Data);
                    var tmp = MealList[SelectedIndex];
                    var tmpDish = await _diningService.GetListOfDishes(tmp.ID, AppConstants.User.GradeID, DateData.ToString("yyyy/MM/dd"));
                    if (tmpDish.Code > 0)
                    {
                        var monAnData = new List<ListOfDishesModel>(tmpDish.Data);
                        var monAn = monAnData[0].MonAn;
                        if (monAn != null)
                        {
                            DishData = new List<DishModel>();
                            var lsMonAn = monAn.Split(',');
                            for (int i = 0; i < lsMonAn.Length; i++)
                            {
                                var chiTietMonAn = await _diningService.GetDishDetail(lsMonAn[i]);
                                if (chiTietMonAn.Code > 0)
                                {
                                    var tmpMonAn = chiTietMonAn.Data.ToList();
                                    DishData.Add(tmpMonAn[0]);
                                    if (DishData == null)
                                    {
                                        IsHaveDish = false;
                                    }
                                    else
                                    {
                                        IsHaveDish = true;
                                    }
                                }
                            }
                        }
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
    }
}