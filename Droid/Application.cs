﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using PushNotification.Plugin;

namespace MeetingPlanner.Droid
{
    [Application]
    public class MeetingPlanner : Application
    {
        public static Context AppContext;

        public MeetingPlanner(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            AppContext = this.ApplicationContext;

            //TODO: Initialize CrossPushNotification Plugin
            //TODO: Replace string parameter with your Android SENDER ID
            //TODO: Specify the listener class implementing IPushNotificationListener interface in the Initialize generic
            CrossPushNotification.Initialize<CrossPushNotificationListener>("<ANDROID SENDER ID>");
            CrossPushNotification.Current.Register();
            //This service will keep your app receiving push even when closed.             
            StartPushService();
        }

        public static void StartPushService()
        {
            AppContext.StartService(new Intent(AppContext, typeof(PushNotificationService)));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public static void StopPushService()
        {
            AppContext.StopService(new Intent(AppContext, typeof(PushNotificationService)));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }
    }
}
