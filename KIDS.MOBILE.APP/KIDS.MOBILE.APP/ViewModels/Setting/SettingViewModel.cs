using Prism.Navigation;

namespace KIDS.MOBILE.APP.ViewModels.Setting
{
    public class SettingViewModel : BaseViewModel
    {
        private string _userName;
        private string _pass;
        private string _passwdOld;
        private string _passwdNew;

        public string PasswdOld
        {
            get => _passwdOld;
            set => SetProperty(ref _passwdOld, value);
        }

        public string PasswdNew
        {
            get => _passwdNew;
            set => SetProperty(ref _passwdNew, value);
        }

        public SettingViewModel()
        {

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