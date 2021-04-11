using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KIDS.MOBILE.APP.Services.Database;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.User
{
    internal class UserProfileViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private TeacherModel _teacher;
        private string _picture;
        private string _name;
        private byte _sex;
        private DateTime _dob;
        private string _email;
        private string _phone;
        private string _address;
        private bool _isLoading;
        private FileResult _fileResult;
        private IDatabaseService _databaseService;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public TeacherModel Teacher
        {
            get => _teacher;
            set => SetProperty(ref _teacher, value);
        }

        private ImageSource _ProfilePicture;
        public ImageSource ProfilePicture
        {
            get => _ProfilePicture;
            set => SetProperty(ref _ProfilePicture, value);
        }

        public ICommand ChangeAvatarCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand UpdateProfileCommand { get; set; }

        public ICommand ChangeImageCommand { get; }

        public UserProfileViewModel(INavigationService navigationService, IUserService userService, IPageDialogService pageDialogService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _userService = userService;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            ChangeAvatarCommand = new Command(ChangeAvatar);
            GoBackCommand = new Command(GoBack);
            UpdateProfileCommand = new Command(async () => await UpdateProfile());
            ChangeImageCommand = new DelegateCommand(async () => await ChangeProfilePicture());
        }

        private void ChangeAvatar()
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            await InitializationProfile();
        }

        private async Task UpdateProfile()
        {
            try
            {
                IsLoading = true;

                var para = new UpdateTeacherModel()
                {
                    TeacherId = AppConstants.User.NguoiSuDung,
                    Picture = _fileResult,
                    Name = Teacher.Name,
                    Sex = Teacher.Sex.ToString(),
                    DOB = Teacher.DOB.Date.ToString("yyyy-MM-dd"),
                    Email = Teacher.Email,
                    Phone = Teacher.Phone,
                    Address = Teacher.Address
                };
                var data = await _userService.UpdateTeacher(para);
                if (data.Code > 0)
                {
                    await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00097, "OK");
                    await _navigationService.NavigateAsync("/LoginPage");
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00060, "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00060, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task InitializationProfile()
        {
            try
            {
                var teacher = await _userService.GetTeacher(AppConstants.User.NguoiSuDung);
                if (teacher != null)
                {
                    if (teacher.Code > 0)
                    {
                        var data = new ObservableCollection<TeacherModel>(teacher.Data);
                        Teacher = data.ElementAt(0);
                        ProfilePicture = new Uri(Teacher.AvatarTeacher);
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

        private void GoBack()
        {
            _navigationService.GoBackAsync();
        }
        private async Task ChangeProfilePicture()
        {
            try
            {

                await _pageDialogService.DisplayActionSheetAsync("Chọn ảnh đại diện", ActionSheetButton.CreateCancelButton("Bỏ qua"), null, ActionSheetButton.CreateButton("Thư viên", async () => await ChonAnh()), ActionSheetButton.CreateButton("Máy ảnh", async () => await DungMayAnh()));
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task DungMayAnh()
        {
            try
            {
                var capture = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
                {
                    Title = "Chụp ảnh"
                });
                if (capture != null)
                {
                    _fileResult = capture;
                    var stream = await capture.OpenReadAsync();
                    ProfilePicture = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task ChonAnh()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
                {
                    Title = "Chọn ảnh"
                });
                if (photo != null)
                {
                    _fileResult = photo;
                    var stream = await photo.OpenReadAsync();
                    ProfilePicture = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}