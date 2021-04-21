using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Animation
{
    public class MarqueeBehavior : Behavior<Label>
    {
        #region Properties

        private Label label { get; set; }
        private bool isStartTimer;

        public double PageWidth
        {
            get { return (double)GetValue(PageWidthProperty); }
            set { SetValue(PageWidthProperty, value); }
        }

        // Using a BindableProperty as the backing store for PageWidth. This enables animation, styling, binding, etc.
        public static readonly BindableProperty PageWidthProperty =
            BindableProperty.Create("PageWidth", typeof(double), typeof(MarqueeBehavior));

        #endregion

        #region Methods

        protected override void OnAttachedTo(Label label)
        {
            base.OnAttachedTo(label);
            this.label = label;
            isStartTimer = true;
            label.BindingContextChanged += StackLayout_BindingContextChanged;
        }

        /// <summary>
        /// This event is invoked when stacklayout binding context is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackLayout_BindingContextChanged(object sender, EventArgs e)
        {
            StartAnimation();
        }

        /// <summary>
        /// This method is used for starting the marquee scrolling animation.
        /// </summary>
        private void StartAnimation()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                label.TranslationX -= 20f;

                if (Math.Abs(label.TranslationX) > PageWidth)
                {
                    label.TranslationX = PageWidth;
                }
                return isStartTimer;
            });
        }

        /// <summary>
        /// This method is used for getting the marquee label content.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="PriorityId"></param>
        /// <returns></returns>
        private Label GetNewButton(string content, int priorityId)
        {
            var button = new Label()
            {
                FontSize = 16,
                Text = content,
                HeightRequest = 45,
                Margin = new Thickness(0, 0, 8, 0),
                Padding = new Thickness(12, 0, 18, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.FromHex("#0040ff"),
                BackgroundColor = GetPriorityColor(priorityId),
                VerticalOptions = LayoutOptions.Center,
            };

            return button;
        }

        /// <summary>
        /// This method is used to return the color based on priority level.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Color GetPriorityColor(int id)
        {
            //if (id == 1)
            //    return Color.FromHex("#FFDCE5");
            //else if (id == 2)
            //    return Color.FromHex("#DBEFFE");
            //else
            //    return Color.FromHex("#FFEADC");
            return Color.White;
        }

        protected override void OnDetachingFrom(Label label)
        {
            label.BindingContextChanged -= StackLayout_BindingContextChanged;
            isStartTimer = false;
            base.OnDetachingFrom(label);
        }

        #endregion
    }
}
