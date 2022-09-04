using System;
using System.IO;
using Android.Content;
using Java.IO;
using Xamarin.Forms;
using System.Threading.Tasks;

using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using AndroidX.Core.Content;
using AndroidX.Core.App;

[assembly: Dependency(typeof(SaveAndroid))]

class SaveAndroid : ISave
{
    //Method to save document as a file in Android and view the saved document
    [Obsolete]
    public async Task Save(string fileName, String contentType, MemoryStream stream)
    {
        string exception = string.Empty;
        string root = null;



        //Get the root path in android device.
        if (Android.OS.Environment.IsExternalStorageEmulated)
        {
            root = Android.OS.Environment.ExternalStorageDirectory.ToString();
        }
        else
            root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


        //Create directory and file
        Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion/");
        myDir.Mkdirs();

        Java.IO.File file = new Java.IO.File(myDir, fileName);

        //Remove if the file exists
        if (file.Exists()) file.Delete();

        try
        {
            FileOutputStream outs = new FileOutputStream(file);
            outs.Write(stream.ToArray());

            outs.Flush();
            outs.Close();

        }
        catch (Exception e)
        {
            exception = e.ToString();
        }
    }
}