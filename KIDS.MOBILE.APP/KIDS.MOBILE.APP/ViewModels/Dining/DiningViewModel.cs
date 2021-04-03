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
using KIDS.MOBILE.APP.Services.Database;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
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
        private IDatabaseService _databaseService;
        private string _thucDon;
        private bool _isOpenFastFeature;
        private IPageDialogService _pageDialogService;

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

        public string ThucDon
        {
            get => _thucDon;
            set => SetProperty(ref _thucDon, value);
        }

        public bool IsOpenFastFeature
        {
            get => _isOpenFastFeature;
            set => SetProperty(ref _isOpenFastFeature, value);
        }

        public ICommand FastFeatureCommand { get; private set; }



        public DiningViewModel(IAttendanceService attendanceService, IDiningService diningService, IDialogService dialogService, IDatabaseService databaseService, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _dialogService = dialogService;
            _diningService = diningService;
            _attendanceService = attendanceService;
            SelectDateCommand = new Command(OpenDatePicker);
            FastFeatureCommand = new AsyncCommand<string>(s => FastFeature(s));

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
                    await QuickComment();
                    break;
                case "2":
                    await DiningComment(key);
                    break;
                default:
                    break;
            }

            IsLoading = false;
        }

        private async Task DiningComment(string key)
        {
            try
            {
                var cmt = StudentData.Where(x => string.IsNullOrEmpty(x.MealComment0)).ToList();
                if (cmt.Any())
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Vẫn còn có con chưa được nhận xét", "OK");
                }
                else
                {
                    var count = 0;
                    foreach (var item in StudentData)
                    {
                        count += await UpdateBreakFast(item);
                    }
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Đã nhận xét " + count + " con thanh công", "OK");
                    IsOpenFastFeature = false;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task QuickComment()
        {
            try
            {
                _dialogService.ShowDialog("QuickCommentDialog", new DialogParameters("key=1"), result =>
                {
                    var para = result.Parameters.GetValue<string>("CommentContent");
                    if (para != null)
                    {
                        foreach (var student in StudentData)
                        {
                            if (string.IsNullOrWhiteSpace(student.MealComment0))
                            {
                                student.MealComment0 = para;
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

        private async Task<int> UpdateBreakFast(AttendanceLeaveModel obj)
        {
            try
            {
                IsLoading = true;
                var update =
                    await _diningService.UpdateDining(obj.ID, _nguoiDung, obj.MealComment0, "", "", "", "", "");
                return update.Data;
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
                    StudentData = new List<AttendanceLeaveModel>(data.Data);
                }

                var tmpDish = await _diningService.GetListOfDishes("", AppConstants.User.GradeID, DateData.ToString("yyyy/MM/dd"));
                if (tmpDish.Code > 0)
                {
                    var monAnData = new List<ListOfDishesModel>(tmpDish?.Data);
                    if (monAnData.Any())
                    {
                        ThucDon = "";
                        var sang = monAnData.FirstOrDefault(x => x.BuaAn == "80cd65d9-d619-4161-a2a6-69a455fc8117");
                        if (sang != null)
                            ThucDon += "Bứa sáng" + "\n\t " + string.Join("\n\t", sang.TenMonAn.Split(',')) + "\n";
                        var trua = monAnData.FirstOrDefault(x => x.BuaAn == "fa1726ce-8736-42e5-bd86-1c44089dd8c6");
                        if (trua != null)
                            ThucDon += "Bứa trưa" + "\n\t " + string.Join("\n\t", trua.TenMonAn.Split(',')) + "\n";
                        var chieuPhu = monAnData.FirstOrDefault(x => x.BuaAn == "2f4ed0f5-a1dd-46ca-9067-78d1a745a638");
                        if (chieuPhu != null)
                            ThucDon += "Bứa chiều phụ" + "\n\t " + string.Join("\n\t", chieuPhu.TenMonAn.Split(',')) + "\n";
                        var chieu = monAnData.FirstOrDefault(x => x.BuaAn == "f4424fae-5ad4-4267-a0b9-36fa32cc79c2");
                        if (chieu != null)
                            ThucDon += "Bứa chiều" + "\n\t " + string.Join("\n\t", chieu.TenMonAn.Split(',')) + "\n";
                        ThucDon?.Replace(" ", "");
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