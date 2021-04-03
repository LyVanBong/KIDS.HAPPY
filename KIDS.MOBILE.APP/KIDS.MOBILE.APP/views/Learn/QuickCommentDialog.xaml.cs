using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KIDS.MOBILE.APP.views.Learn
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickCommentDialog
    {
        public QuickCommentDialog()
        {
            InitializeComponent();
        }

        private void RadioButton_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var rd = sender as RadioButton;
            if (rd != null)
            {
                edcomment.Text = rd.Content?.ToString();
            }
        }
    }
}