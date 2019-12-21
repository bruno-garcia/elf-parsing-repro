using System.IO;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using ClassLib;

namespace SymbolCollector.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            foreach (var item in Directory.GetFiles("/system/lib"))
            {
                Class1.Test(item);
            }
        }
    }
}
