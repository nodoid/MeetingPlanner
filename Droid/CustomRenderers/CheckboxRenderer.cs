﻿using System;
using System.ComponentModel;
using Android.Widget;
using MeetingPlanner;
using MeetingPlanner.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Checkbox), typeof(CheckboxRenderer))]
namespace MeetingPlanner.Droid
{
    /// <summary>
    /// Class CheckBoxRenderer.
    /// </summary>
    public class CheckboxRenderer : ViewRenderer<Checkbox, Android.Widget.CheckBox>
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        protected override CheckBox CreateNativeControl()
        {
            return new Android.Widget.CheckBox(Forms.Context);
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Checkbox> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var checkBox = new Android.Widget.CheckBox(this.Context);
                if (Element.WidthRequest >= 0)
                {
                    checkBox.SetWidth((int)Element.WidthRequest);
                    checkBox.SetHeight((int)Element.WidthRequest);
                }
                checkBox.CheckedChange += CheckBoxCheckedChange;
                SetNativeControl(checkBox);
            }

            if (e.NewElement != null)
            {
                if (e.NewElement.WidthRequest >= 0)
                {
                    Control.SetHeight((int)e.NewElement.WidthRequest);
                    Control.SetWidth((int)e.NewElement.WidthRequest);
                }
                Control.Checked = e.NewElement.Checked;
                Control.Enabled = e.NewElement.IsEnabled;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    break;
            }
        }

        /// <summary>
        /// CheckBoxes the checked change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Android.Widget.CompoundButton.CheckedChangeEventArgs"/> instance containing the event data.</param>
        void CheckBoxCheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            Element.Checked = e.IsChecked;
        }
    }
}
