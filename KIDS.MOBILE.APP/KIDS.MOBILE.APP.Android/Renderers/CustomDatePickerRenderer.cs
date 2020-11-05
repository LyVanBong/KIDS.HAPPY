using KIDS.MOBILE.APP.Droid.Renderers;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]

namespace KIDS.MOBILE.APP.Droid.Renderers
{
    internal class CustomDatePickerRenderer : DatePickerRenderer
    {
        public static void Init()
        {
        }

        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }

        public CustomDatePicker ElementV2 => Element as CustomDatePicker;

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.LightGray);
                Control.SetBackgroundDrawable(gd);
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}