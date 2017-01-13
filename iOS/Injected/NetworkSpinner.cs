using System;
using Foundation;
using UIKit;
using MeetingPlanner.iOS;
using Xamarin.Forms;
using CoreGraphics;

[assembly: Dependency(typeof(NetworkSpinner))]
namespace MeetingPlanner.iOS
{
    public class NetworkSpinner : INetworkSpinner
    {
        static UIView spinView;
        static UILabel txtMessage;

        public void ChangeMessage(string newMessage)
        {
            if (spinView != null)
            {
                using (var pool = new NSAutoreleasePool())
                {
                    pool.InvokeOnMainThread(delegate ()
                    {
                        txtMessage.Text = newMessage;
                    });
                }
            }
        }

        public void Spinner(bool on, string title, string message)
        {
            var lblTitle = new UILabel(new CGRect(89, 7, 192, 26))
            {
                TextColor = UIColor.Black,
                Font = UIFont.SystemFontOfSize((nfloat)22),
                Text = title
            };

            var spinSpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge)
            {
                Frame = new CGRect(25, 19, 37, 37),
                AutoresizingMask = UIViewAutoresizing.All,
            };

            if (spinView == null)
            {
                var centerX = (App.ScreenSize.Width / 2) - (286 / 2);
                var centerY = (App.ScreenSize.Height / 2) - (75 / 2);
                spinView = new UIView(new CGRect(centerX, centerY, 286, 75))
                {
                    BackgroundColor = UIColor.LightGray,
                };
                spinView.Layer.BorderWidth = 0;
                spinView.Layer.CornerRadius = 4f;
                txtMessage = new UILabel(new CGRect(89, 41, 185, 21))
                {
                    Text = message
                };
                spinView.AddSubviews(new UIView[] { spinSpinner, lblTitle, txtMessage });
            }

            spinView.BringSubviewToFront(spinSpinner);
            try
            {
                UIApplication.SharedApplication.KeyWindow.RootViewController.Add(spinView);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("Exception in netspinner - {0}--{1}", ex.Message, ex.InnerException);
#endif
            }
            if (on)
                spinSpinner.StartAnimating();
            else
                spinView.RemoveFromSuperview();

            using (var pool = new NSAutoreleasePool())
            {
                pool.InvokeOnMainThread(delegate ()
                {
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = on;
                });
            }
        }
    }
}

