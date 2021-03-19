using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.User;
using Microsoft.AppCenter.Crashes;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public UserProfileViewModel(INavigationService navigationService, IUserService userService, IPageDialogService pageDialogService)
        {
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
                    Picture = Teacher.Picture,
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
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                return;
            }
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Could not get the image, please try again.", "Ok");
                return;
            }
            ProfilePicture = ImageSource.FromStream(() => selectedImageFile.GetStream());
            var tempUrl = Teacher.Picture;
            Teacher.Picture = ImageSourceToBase64(ProfilePicture);
            await UpdateProfile();
            //var result = await _userService.UpdateInfoUser(User);
            //if (result == null || result.Data < 0)
            //{
            //    await _pageDialogService.DisplayAlertAsync(Resource._00002, Resource._00063, "OK");
            //    User.Picture = tempUrl;
            //    ProfilePicture = ImageSource.FromUri(new Uri(Teacher.AvatarTeacher));
            //}
        }
    }
}