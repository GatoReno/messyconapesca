using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;

namespace SqlieTest_x.Droid
{
    [Activity(Label = "Conapesca Service", Icon = "@drawable/iko", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    //[assembly: UsesPermission(Manifest.Permission.AccessFineLocation)]
    //[assembly: UsesPermission(Manifest.Permission.AccessCoarseLocation)]
    //[assembly: UsesPermission(Manifest.Permission.Internet)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

