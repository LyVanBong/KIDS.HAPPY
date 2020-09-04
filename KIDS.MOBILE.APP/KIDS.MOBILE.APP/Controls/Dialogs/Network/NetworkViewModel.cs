using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.ViewModels;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Dialogs.Network
{
    public class NetworkViewModel : BaseViewModel
    {
        private INavigationService _navigationService;

        public NetworkViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Task.Run(() =>
            {
                AppConstants.IsOpenPopupNetwork = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (!AppConstants.HasInternet)
                        return true;
                    else
                    {
                        AppConstants.IsOpenPopupNetwork = false;
                        _navigationService.GoBackAsync();
                        return false;
                    }
                });
            });
        }
    }
}