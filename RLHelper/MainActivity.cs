using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using RLHelper.Fragments;

namespace RLHelper
{
    [Activity(Label = "RLHelper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetTheme(Resource.Style.Theme_AppCompat_Light_NoActionBar);
            SetContentView(Resource.Layout.Main);

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.currentlayoutContent, new WordAnalysisFrag(), "AnalysisFragment");
            trans.Commit();
        }
    }
}

