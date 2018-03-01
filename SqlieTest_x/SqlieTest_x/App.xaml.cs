using Plugin.Geolocator;
using Plugin.LocalNotifications;
using Plugin.Vibrate;
using SQLite;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SqlieTest_x
{
    public partial class App : Application
    {

        public MemberDatabase memberDatabase;
        public Member member;
        public SQLiteConnection conn;

        public App()
        {
            InitializeComponent();

            MainPage =

             new NavigationPage(new SplashPage());
            //  memberDatabase = new MemberDatabase();
            //  var members = memberDatabase.GetMembers();

        }

        protected override void OnStart()
        {
            // Handle when your app starts


            //Sqli instance
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();

            int RowCount = 0;
            int membcount = members.Count();
            RowCount = Convert.ToInt32(membcount);
            if (RowCount > 0)
            {
                SendLocation();
            }


        }

        protected override void OnSleep()
        {

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan_CheckLocation = TimeSpan.FromSeconds
            (15);
            var periodTimeSpan_SendLocation = TimeSpan.FromMinutes
            (5);

            var timer = new System.Threading.Timer((e) =>
                {
                    CheckLocation();
                }, null, startTimeSpan, periodTimeSpan_CheckLocation);


            var timer_ = new System.Threading.Timer((e) =>
            {
                SendLocation();
            }, null, startTimeSpan, periodTimeSpan_SendLocation);


        }

        public  void CheckLocation() {

            if (!CrossGeolocator.Current.IsGeolocationEnabled)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");

                var v = CrossVibrate.Current;
                v.Vibration(TimeSpan.FromSeconds(2));
                CrossLocalNotifications.Current.Show("Error!!!", "Activa tu GPS!!!", 101, DateTime.Now);
            }
            if (!CrossConnectivity.Current.IsConnected)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");

                var v = CrossVibrate.Current;
                v.Vibration(TimeSpan.FromSeconds(2));
                CrossLocalNotifications.Current.Show("Error!!!", "Activa tus datos!!!", 101, DateTime.Now);
            }
           

        }

        public  async void SendLocation() {

           PageNav nav = new PageNav();


            if (!CrossGeolocator.Current.IsGeolocationEnabled)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");
                var v = CrossVibrate.Current;
                v.Vibration(TimeSpan.FromSeconds(2));
              
                CrossLocalNotifications.Current.Show("Error!!!", "Activa tu GPS!!!", 101, DateTime.Now);
            }
            else { 
            await nav.RetriveLocation();
                CrossLocalNotifications.Current.Show("Estas en horas de trabajo", 
                "Registro de ubicación exitosos", 101, DateTime.Now);

                Debug.WriteLine("OnSleep zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz I'm sleeping"+
             "please ignore this, that's not my id zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz"
              );

            }

        }

        
        protected override void OnResume ()
		{
            // Handle when your app resumes
            SendLocation();
            Debug.WriteLine("I've been Remused"
             );
        }
	}
}
