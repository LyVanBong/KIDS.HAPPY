using System;
using System.Threading.Tasks;
using System.Windows.Input;
using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Helpers;
using KIDS.MOBILE.APP.Services.Database;
using KIDS.MOBILE.APP.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace KIDS.MOBILE.APP.ViewModels.ChangePass
{
    class ChangePasswordViewModel : BaseViewModel
    {
        private bool _isLoading;
        private string _retypeNewPass;
        private string _newPassword;
        private string _oldPassword;
        private string _userName;
        private string _pass;
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private INavigationService _navigationService;
        private IDatabaseService _databaseService;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand UpdatePassCommand { get; private set; }

        public string RetypeNewPass
        {
            get => _retypeNewPass;
            set => SetProperty(ref _retypeNewPass, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        public ChangePasswordViewModel(IPageDialogService pageDialogService, IUserService userService, INavigationService navigationService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            UpdatePassCommand = new DelegateCommand(UpdatePasswd);
        }
        private async void UpdatePasswd()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (OldPassword != null && NewPassword != null && RetypeNewPass != null)
                {
                    if (HashFunctionHelper.GetHashCode(OldPassword, 1) != _pass)
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Mật khẩu hiện tại chưa chính xác", "OK");
                        OldPassword = null;
                    }
                    else if (NewPassword != RetypeNewPass)
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Mật khẩu mới chưa trùng nhau", "OK");
                    }
                    else
                    {
                        var change = await _userService.ChangePasswd(_userName,
                            HashFunctionHelper.GetHashCode(NewPassword, 1));
                        if (change != null && change.Code > 0 && change.Data == "1")
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Đổi mật khẩu thành công", "OK");
                            await _databaseService.DeleteAccontUser();
                            AppConstants.User = null;
                            if (Preferences.ContainsKey(AppConstants.SaveAccountUser))
                                Preferences.Remove(AppConstants.SaveAccountUser);
                            await _navigationService.NavigateAsync("/LoginPage");
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Đổi mật khẩu chưa thành công", "OK");
                            OldPassword = null;
                            NewPassword = null;
                            RetypeNewPass = null;
                        }
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Vui lòng nhập đầy đủ thông tin", "OK");
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
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                _userName = parameters.GetValue<string>("user");
                _pass = parameters.GetValue<string>("pass");
            }
        }
    }
}
