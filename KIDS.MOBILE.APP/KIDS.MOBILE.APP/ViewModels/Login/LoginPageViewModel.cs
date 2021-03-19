using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Helpers;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.Database;
using KIDS.MOBILE.APP.Services.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace KIDS.MOBILE.APP.ViewModels.Login
{
    public class LoginPageViewModel : BaseViewModel
    {
        private ILoginService _loginService;
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private bool _isSaveAccountUser;
        private bool _isCheckLogin;
        public ICommand LoginAppCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; private set; }
        public string UserName { get; set; }
        public string Passwd { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// kiem tra luu tai khoan
        /// </summary>
        public bool IsSaveAccountUser
        {
            get => _isSaveAccountUser;
            set => SetProperty(ref _isSaveAccountUser, value);
        }

        private IDatabaseService _databaseService;

        public LoginPageViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _loginService = loginService;
            _navigationService = navigationService;
            LoginAppCommand = new DelegateCommand(LoginApp);
            ForgotPasswordCommand = new DelegateCommand(OnForgotPasswordClicked);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsSaveAccountUser = true;
            CheckLogin();
        }

        private async void LoginApp()
        {
            try
            {
                if (IsLoading)
                    return;
                IsLoading = true;
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Passwd))
                {
                    var pass = _isCheckLogin ? Passwd : HashFunctionHelper.GetHashCode(Passwd, 1);
                    var data = await _loginService.LogiAppByUserPwd(UserName,
                        pass);
                    if (data != null)
                    {
                        if (data.Code == 0 && data.Data.IsTeacher.Equals("1"))
                        {
                            await CheckSaveAccount(data.Data);
                            await _navigationService.NavigateAsync("/MainPage?selectedTab=home");
                        }
                        else
                            await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00005, "OK");
                    }
                    else
                        await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00005, "OK");
                }
                else
                    await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00006, "OK");
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

        /// <summary>
        /// luu tai khoan dang nhap
        /// </summary>
        /// <returns></returns>
        private async Task CheckSaveAccount(UserModel user)
        {
            try
            {
                if (IsSaveAccountUser && user != null)
                {
                    Preferences.Set(AppConstants.SaveAccountUser, true);
                }
                else
                {
                    if (Preferences.ContainsKey(AppConstants.SaveAccountUser))
                        Preferences.Remove(AppConstants.SaveAccountUser);
                }
                await _databaseService.UpdateAccountUser(user);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        /// <summary>
        /// login
        /// </summary>
        private async void CheckLogin()
        {
            if (Preferences.ContainsKey(AppConstants.SaveAccountUser))
                if (Preferences.Get(AppConstants.SaveAccountUser, false))
                {
                    _isCheckLogin = true;
                    var user = await _databaseService.GetAccountUser();
                    UserName = user.NickName;
                    Passwd = user.Password;
                    await Task.Delay(TimeSpan.FromMilliseconds(1500));
                    LoginApp();
                }
        }

        private async void OnForgotPasswordClicked()
        {
            await _pageDialogService.DisplayAlertAsync("Quên mật khẩu ?", "Để lấy lại mật khẩu, vui lòng liên hệ với nhà trường hoặc admin HappyKids", "OK");
        }
    }
}