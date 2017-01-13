using System.ComponentModel;
using System.Globalization;
using MeetingPlanner.Languages;
using Plugin.Connectivity;
using SQLite.Net.Interop;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeetingPlanner
{
    public class App : Application
    {
        public static Size ScreenSize { get; set; }

        public static App Self { get; private set; }

        public string DBConnection { get; set; }

        public SQL DBManager { get; private set; }

        public ISQLitePlatform SQLitePlatform { get; private set; }

        public IUserSettings UserSettings { get; set; } = DependencyService.Get<IUserSettings>();

        public INetworkSpinner NetSpinner { get; set; } = DependencyService.Get<INetworkSpinner>();

        public UIChangedEvent MessageEvents { get; set; }

        public string ConnectionString { get; set; }

        public string ContentPath { get; set; }

        public string PhonePlatform { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool PanelShowing { get; set; } = false;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isConnected;

        public bool IsConnected
        {
            get { return isConnected; }
            private set
            {
                isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public App()
        {
            App.Self = this;

            var netLanguage = DependencyService.Get<ILocalise>().GetCurrent();
            Langs.Culture = new CultureInfo(netLanguage);

            ConnectionString = DependencyService.Get<ISQLite>().GetConnectionString();
            App.Self.SQLitePlatform = DependencyService.Get<ISQLite>().GetPlatform();
            DependencyService.Get<ILocalise>().SetLocale();

            DBManager = new SQL();
            DBManager.SetupDB();

            IsConnected = CrossConnectivity.Current.IsConnected;

            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                IsConnected = args.IsConnected;
            };

            MessageEvents = new UIChangedEvent();

            /*
             #if DEBUG
                        new TestData();
            #endif
            */

            // The root page of your application
            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
