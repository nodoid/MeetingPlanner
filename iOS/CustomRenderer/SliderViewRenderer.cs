﻿using MeetingPlanner;
using MeetingPlanner.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(SliderView), typeof(SliderViewRenderer))]
namespace MeetingPlanner.iOS
{
    public class SliderViewRenderer : ViewRenderer
    {
        SliderView sliderView;
        int currentViewIndex = 0;
        UISwipeGestureRecognizer rightGesture, leftGesture;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                sliderView = e.NewElement as SliderView;
                sliderView.Children[currentViewIndex].HeightRequest = sliderView.Height;
                sliderView.Children[currentViewIndex].WidthRequest = sliderView.Width;
                sliderView.Children[currentViewIndex].BackgroundColor = sliderView.BackgroundColor;
            }

            rightGesture = new UISwipeGestureRecognizer(swipe =>
            {
                if (sliderView.Children.Count > currentViewIndex + 1)
                {
                    currentViewIndex++;

                    sliderView.CurrentView = sliderView.Children[currentViewIndex];
                    sliderView.CurrentView.HeightRequest = sliderView.Height;
                    sliderView.CurrentView.WidthRequest = sliderView.Width;
                    sliderView.CurrentView.BackgroundColor = sliderView.BackgroundColor;

                    TranslateToCurrentView("Left");
                }
            })
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };

            leftGesture = new UISwipeGestureRecognizer(swipe =>
            {
                if (currentViewIndex != 0)
                {
                    currentViewIndex--;
                    sliderView.CurrentView = sliderView.Children[currentViewIndex];
                    sliderView.CurrentView.HeightRequest = sliderView.Height;
                    sliderView.CurrentView.WidthRequest = sliderView.Width;
                    sliderView.CurrentView.BackgroundColor = sliderView.BackgroundColor;

                    TranslateToCurrentView("Right");
                }
            })
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };

            AddGestureRecognizer(rightGesture);
            AddGestureRecognizer(leftGesture);
        }

        protected override void Dispose(bool disposing)
        {
            RemoveGestureRecognizer(rightGesture);
            RemoveGestureRecognizer(leftGesture);
            leftGesture.Dispose();
            rightGesture.Dispose();
        }

        public async Task TranslateToCurrentView(string direction)
        {
            var initialLayoutRect = new Rectangle(
                0,
                0,
                sliderView.Width,
                sliderView.Height
            );

            var dotRect = new Rectangle(
                x: sliderView.ViewScreen.Width / 2 - (sliderView.DotStack.Children.Count * 15) / 2,
                y: sliderView.ViewScreen.Height - 15,
                width: sliderView.DotStack.Children.Count * 15,
                height: 10
            );

            sliderView.ViewScreen.Children.Remove(sliderView.DotStack);
            sliderView.UpdateDots();

            foreach (var dot in sliderView.DotStack.Children)
            {
                dot.Opacity = 0.5;
                if (dot.StyleId == currentViewIndex.ToString())
                    dot.Opacity = 1;
            }

            switch (direction)
            {
                case "Right":
                    initialLayoutRect.X = -sliderView.ParentView.Width;
                    sliderView.ViewScreen.Children.Add(sliderView.CurrentView, initialLayoutRect);
                    sliderView.ViewScreen.Children.Add(sliderView.DotStack, dotRect);

                    await sliderView.CurrentView.TranslateTo(sliderView.ParentView.Width, 0, sliderView.TransitionLength);
                    sliderView.ViewScreen.Children.Remove(sliderView.Children[currentViewIndex + 1]);
                    break;
                case "Left":
                    initialLayoutRect.X = sliderView.ParentView.Width;

                    sliderView.ViewScreen.Children.Add(sliderView.CurrentView, initialLayoutRect);
                    sliderView.ViewScreen.Children.Add(sliderView.DotStack, dotRect);

                    await sliderView.CurrentView.TranslateTo(-sliderView.ParentView.Width, 0, sliderView.TransitionLength);
                    sliderView.ViewScreen.Children.Remove(sliderView.Children[currentViewIndex - 1]);
                    break;
            }
        }
    }
}

