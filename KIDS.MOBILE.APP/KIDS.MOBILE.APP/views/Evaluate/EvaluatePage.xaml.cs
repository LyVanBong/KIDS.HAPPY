using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KIDS.MOBILE.APP.views.Evaluate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvaluatePage : ContentPage
    {
        public EvaluatePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SfDataGrid.ClearSelection();
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        var safeInsets = On<iOS>().SafeAreaInsets();
            //        safeInsets.Bottom = 0;
            //        safeInsets.Left = 0;
            //        safeInsets.Right = 0;
            //        Padding = safeInsets;
            //    });
            //}
        }

        private void SfDataGrid_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            SfDataGrid.ClearSelection();
        }
    }
}