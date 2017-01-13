using System;
using System.ComponentModel;
using CoreGraphics;
using MeetingPlanner;
using MeetingPlanner.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]
namespace MeetingPlanner.iOS
{
    public class CheckboxRenderer : ViewRenderer<Checkbox, M13Checkbox>
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public new static void Init()
        {
            var temp = DateTime.Now;
        }

        private CGRect _originalBounds;

        /// <summary>
        /// Handles the Element Changed event
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            BackgroundColor = Element.BackgroundColor.ToUIColor();
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var width = (double)Constants.CheckboxDefaultHeight;
                    if (Element.WidthRequest >= 0)
                    {
                        width = Element.WidthRequest;
                    }
                    var checkBox = new M13Checkbox(new CGRect(0, 0, width, width));
                    checkBox.Bounds = new CGRect(0, 0, width, width);
                    checkBox.CheckedChanged += (s, args) => Element.Checked = args.Checked;
                    SetNativeControl(checkBox);

                    // Issue with list rendering
                    _originalBounds = checkBox.Bounds;
                }
                Control.SetCheckState(e.NewElement.Checked
                    ? CheckboxState.Checked : CheckboxState.Unchecked);
                Control.SetEnabled(e.NewElement.IsEnabled);
                Control.Bounds = _originalBounds;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            switch (e.PropertyName)
            {
                //case "IsVisible":
                //    Control.Hidden = Element.IsVisible;
                //    break;
                case "IsEnabled":
                    Control.SetEnabled(Element.IsEnabled);
                    break;
                case "Checked":
                    Control.SetCheckState(Element.Checked ? CheckboxState.Checked : CheckboxState.Unchecked);
                    break;

            }
        }
    }
}
