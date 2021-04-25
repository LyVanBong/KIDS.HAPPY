using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using KIDS.MOBILE.APP.Droid.Service;
using LabelHtml.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;

namespace KIDS.MOBILE.APP.Droid
{
    [Activity(Label = "HK Teacher", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            OtherLibraries(bundle);
            LoadApplication(new App(new DroidInitializer()));
        }

        private void OtherLibraries(Bundle bundle)
        {
            Rg.Plugins.Popup.Popup.Init(this);
            HtmlLabelRenderer.Initialize();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            CrossCurrentActivity.Current.Init(this, bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            MultiMediaPickerService.SharedInstance.OnActivityResult(requestCode, resultCode, data);
        }
    }

    public class DroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}