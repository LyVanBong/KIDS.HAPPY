using System;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace KIDS.MOBILE.APP.views.Setting
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<iOS>().SetUseSafeArea(true);
            var safeInsets = On<iOS>().SafeAreaInsets();
            safeInsets.Bottom = -20;
            Padding = safeInsets;
        }

        private void AnimationTap_OnTapped(object sender)
        {
            var frame = sender as Frame;
            if (frame != null)
            {
                frame.BackgroundColor = Color.FromHex("#4d80e4");
                Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
                {
                    frame.BackgroundColor = Color.Transparent;
                    return false;
                });
            }
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            try
            {
                AnimationTap_OnTapped(sender);
                AppInfo.ShowSettingsUI();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        //private void TapGestureRecognizer_OnTapped2(object sender, EventArgs e)
        //{
        //    AnimationTap_OnTapped(sender);
        //    AppInfo.ShowSettingsUI();
        //}
    }
}
