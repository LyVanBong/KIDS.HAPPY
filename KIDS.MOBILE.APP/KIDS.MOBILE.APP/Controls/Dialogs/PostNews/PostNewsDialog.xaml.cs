using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KIDS.MOBILE.APP.Controls.Dialogs.PostNews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostNewsDialog
    {
        public PostNewsDialog()
        {
            InitializeComponent();
        }

        private void HighLightBackgroundColor_OnTapped(object sender, EventArgs e)
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
    }
}