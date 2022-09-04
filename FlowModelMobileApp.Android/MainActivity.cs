
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using AndroidX.Core.Content;
using Android;
using AndroidX.Core.App;
using Xamarin.Forms;
using System;

namespace FlowModelMobileApp.Droid
{
   [Activity(Label = "FlowModelMobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
      ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                             ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
   public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
   {
      protected override void OnCreate(Bundle savedInstanceState)
      {
         base.OnCreate(savedInstanceState);

         Xamarin.Essentials.Platform.Init(this, savedInstanceState);
         global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
         string fileNameUsers = "users.db";
         string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
         string completePathUsers = Path.Combine(folderPath, fileNameUsers);
         string fileNameModel = "FlowModel.db";
         folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
         string completePathFlowModel = Path.Combine(folderPath, fileNameModel);

         LoadApplication(new App(completePathUsers, completePathFlowModel));
            if (ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions((Android.App.Activity)Forms.Context, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
            }
            else if(ContextCompat.CheckSelfPermission(Forms.Context, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions((Android.App.Activity)Forms.Context, new String[] { Manifest.Permission.ReadExternalStorage }, 1);
            }
        }

      public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
         [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
      {
         Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

         base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      }
        

    }
}