using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Text;
using KIDS.MOBILE.APP.Controls.Renderers;
using KIDS.MOBILE.APP.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]

namespace KIDS.MOBILE.APP.Droid.Controls
{
    public class CustomEditorRenderer : EditorRenderer
    {
        [System.Obsolete]
        public CustomEditorRenderer()
        {
        }

        [System.Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.Black));
            }
        }
    }
}