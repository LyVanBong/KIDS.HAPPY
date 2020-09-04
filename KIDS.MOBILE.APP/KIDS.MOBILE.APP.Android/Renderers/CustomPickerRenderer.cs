using Android.Content;
using Android.Graphics.Drawables;
using KIDS.MOBILE.APP.Controls.CustomRenderers;
using KIDS.MOBILE.APP.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]

namespace KIDS.MOBILE.APP.Droid.Renderers
{
    internal class CustomPickerRenderer : PickerRenderer
    {
        public static void Init()
        {
        }

        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        public CustomPicker ElementV2 => Element as CustomPicker;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            var picker = e.NewElement;

            if (Control != null)
            {
                picker.TextColor = Element.TextColor;
                picker.BackgroundColor = Element.BackgroundColor;
                Control.SetHintTextColor(Android.Graphics.Color.White);
                Control.SetSingleLine(true);
                //remover border
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.LightGray);
                Control.SetBackgroundDrawable(gd);
            }
        }
    }
}