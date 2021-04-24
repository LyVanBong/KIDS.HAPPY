using KIDS.MOBILE.APP.Services.PickMedia;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace KIDS.MOBILE.APP.views.Album
{
    public partial class AddPhotoPage : ContentPage
    {
        public AddPhotoPage()
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, ObservableCollection<MediaFile>>((App)Xamarin.Forms.Application.Current, "PickPhotos");
        }
    }
}
