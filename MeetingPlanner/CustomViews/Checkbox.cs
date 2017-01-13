using System;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class CheckedChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs"/> class.
        /// </summary>
        /// <param name = "chkd"></param>
        public CheckedChangedEventArgs(bool chkd)
        {
            this.IsChecked = chkd;
        }

        /// <summary>
        /// Gets the value of the event argument
        /// </summary>
        public bool IsChecked { get; private set; }
    }

    /// <summary>
    /// The check box.
    /// </summary>
    public class Checkbox : View
    {
        /// <summary>
        /// The checked state property.
        /// </summary>
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create("Checked",
                typeof(bool),
                typeof(Checkbox),
                false, BindingMode.TwoWay, propertyChanged: OnCheckedPropertyChanged);

#if DEBUG
        /// <summary>
        /// The checked state property.
        /// </summary>
        public static readonly BindableProperty TestProperty =
            BindableProperty.Create("Test",
                typeof(string),
                typeof(Checkbox),
                "Test", BindingMode.TwoWay);

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public string Test
        {
            get
            {
                return (string)GetValue(TestProperty);
            }

            set
            {
                SetValue(TestProperty, value);
            }
        }
#endif

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {
            get
            {
                return (bool)GetValue(CheckedProperty);
            }

            set
            {
                if (this.Checked != value)
                {
                    SetValue(CheckedProperty, value);
                    if (CheckedChanged != null)
                        CheckedChanged.Invoke(this, new CheckedChangedEventArgs(value));
                }
            }
        }

        /// <summary>
        /// The checked changed event.
        /// </summary>
        public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

        /// <summary>
        /// Called when [checked property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">if set to <c>true</c> [oldvalue].</param>
        /// <param name="newvalue">if set to <c>true</c> [newvalue].</param>
        private static void OnCheckedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var checkBox = (Checkbox)bindable;
            checkBox.Checked = (bool)newvalue;
        }
    }
}
