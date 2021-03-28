using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace KIDS.MOBILE.APP.views.Album
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumDetailPage : ContentPage
    {
        public AlbumDetailPage()
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
    }
}