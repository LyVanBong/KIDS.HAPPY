using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace KIDS.MOBILE.APP.Controls.Dialogs.Network
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NetworkPage : PopupPage
    {
        public NetworkPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}