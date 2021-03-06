﻿using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Constants
    {
        public static Color NELFTGreen { get; private set; } = Color.FromRgb(99, 179, 46);

        public static Color NELFTYellow { get; private set; } = Color.FromRgb(254, 204, 0);

        public static Color NELFTOrange { get; private set; } = Color.FromRgb(244, 152, 0);

        public static Color NELFTMagenta { get; private set; } = Color.FromRgb(193, 0, 112);

        public static Color NELFTBlue { get; private set; } = Color.FromRgb(0, 113, 187);

        public static Color LightGrey { get; private set; } = Color.FromRgb(245, 245, 245);

        public static double ButtonFontSize = 24;

        public static double SmallEntryFontSize = Device.RuntimePlatform == Device.Android ? 14 : 12;

        public static double HeadlineFontSize = Device.RuntimePlatform == Device.Android ? 28 : 24;

        public static double GeneralFontSize = Device.RuntimePlatform == Device.Android ? 16 : 14;


        public static double SubHeadingFontSize = Device.RuntimePlatform == Device.Android ? 20 : 18;

        public static string BaseUrl = "http://www.all-the-johnsons.co.uk";
        public static string WorkplaceUrl = "http://www.all-the-johnsons.co.uk/php";


        public static string BackgroundFilename = "background_75pc.png";

        public static string Version = "0.05, 06/10/2016";
    }
}

