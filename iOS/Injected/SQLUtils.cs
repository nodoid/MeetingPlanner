﻿using System;
using System.IO;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using MeetingPlanner.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(SQLUtils))]
namespace MeetingPlanner.iOS
{
    public class SQLUtils : ISQLite
    {
        public string GetConnectionString()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var pConnectionString = Path.Combine(documents, "meetplanner.db");
            var connectionString = string.Format("{0}; New=true; Version=3;PRAGMA locking_mode=EXCLUSIVE; PRAGMA journal_mode=WAL; PRAGMA cache_size=20000; PRAGMA page_size=32768; PRAGMA synchronous=off", pConnectionString);
            return connectionString;
        }

        public ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformIOS();
        }
    }
}