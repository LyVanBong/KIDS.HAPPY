using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.Database;
using KIDS.MOBILE.APP.Services.User;
using KIDS.MOBILE.APP.views.User;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KIDS.MOBILE.APP.views.Setting;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Menu
{
    public class MenuViewModel : BaseViewModel, IActiveAware
    {
        private UserModel _userData;
        private bool _isActive;
        private TeacherModel _teacher;

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

        public UserModel UserData
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, IsActiveChange);
        }

        private bool _isGoToProfile;

        public ICommand LogOutCommand { get; set; }

        public event EventHandler IsActiveChanged;

        private INavigationService _navigationService;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private bool _isLoading;

        public ICommand ProfileCommand { get; set; }
        public ICommand SelectFeatureCommad { get; private set; }

        public MenuViewModel(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService pageDialogService, IUserService userService)
        {
            _userService = userService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _navigationService = navigationService;
            LogOutCommand = new Command(async () => await LogOut());
            ProfileCommand = new Command(async () => await Profile());
            SelectFeatureCommad = new AsyncCommand<string>(async (key) => await SelectFeature(key));
        }

        private async Task SelectFeature(string key)
        {
            try
            {
                if (IsLoading || key == null) return;
                IsLoading = true;
                switch (key)
                {
                    case "0":
                        await _navigationService.NavigateAsync("SettingPage?pass=" + UserData.Password+"&user="+UserData.NickName,null,true);
                        break;

                    default:
                        break;
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

        private async Task Profile()
        {
            if (_isGoToProfile)
                return;
            _isGoToProfile = true;
            await _navigationService.NavigateAsync(nameof(UserProfilePage), useModalNavigation: true);
            _isGoToProfile = false;
        }

        private async Task LogOut()
        {
            var answer = await _pageDialogService.DisplayAlertAsync(AppResources._00047, AppResources._00050, AppResources._00048, AppResources._00049);
            if (answer)
            {
                await _databaseService.DeleteAccontUser();
                AppConstants.User = null;
                if (Preferences.ContainsKey(AppConstants.SaveAccountUser))
                    Preferences.Remove(AppConstants.SaveAccountUser);
                await _navigationService.NavigateAsync("/LoginPage");
            }
        }

        private void IsActiveChange()
        {
            if (IsActive)
            {
                InitializationMenu();
            }
            else
            {
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        private async void InitializationMenu()
        {
            IsLoading = true;
            UserData = AppConstants.User;
            try
            {
                var teacher = await _userService.GetTeacher(AppConstants.User.NguoiSuDung);
                if (teacher != null)
                {
                    if (teacher.Code > 0)
                    {
                        var data = new ObservableCollection<TeacherModel>(teacher.Data);
                        Teacher = data.ElementAt(0);
                    }
                    else
                    {
                        Teacher = new TeacherModel();
                    }
                }
                else
                    Teacher = new TeacherModel();
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