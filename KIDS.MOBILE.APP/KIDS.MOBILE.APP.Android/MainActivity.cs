﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using LabelHtml.Forms.Plugin.Droid;
using Lottie.Forms.Droid;
using Plugin.Media;
using Prism;
using Prism.Ioc;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Droid
{
    [Activity(Label = "Happy Kids", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            await CrossMedia.Current.Initialize();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            OtherLibraries(savedInstanceState);
            LoadApplication(new App(new DroidInitializer()));
        }

        private void OtherLibraries(Bundle bundle)
        {
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            HtmlLabelRenderer.Initialize();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            AnimationViewRenderer.Init();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class DroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}