using System;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KIDS.MOBILE.APP.views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void AnimationTap_OnTapped(object sender, EventArgs e)
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

        private async void AnimationTap_OnTapped_Info(object sender, EventArgs e)
        {
            try
            {
                AnimationTap_OnTapped(sender, e);
                await Browser.OpenAsync("http://hkids.edu.vn/", new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("#1976D2"),
                    PreferredControlColor = Color.FromHex("#fff"),
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
    }
}