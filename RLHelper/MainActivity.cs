using Android.App;
using Android.Widget;
using Android.OS;

namespace RLHelper
{
    [Activity(Label = "RLHelper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        PageReceiver parser;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            parser = new PageReceiver();
            parser.OnPageParse += Parser_OnPageParser;
            parser.OnExceptionThrow += Parser_OnExceptionThrow;

            Button handleButton = FindViewById<Button>(Resource.Id.button1);

            handleButton.Click += HandleButton_Click;
        }

        private void HandleButton_Click(object sender, System.EventArgs e)
        {
            EditText textInput = FindViewById<EditText>(Resource.Id.editText1);
            string text = textInput.Text;

           
            parser.Start(text);
        }

        private void Parser_OnPageParser(string resp)
        {
            TextView tw = FindViewById<TextView>(Resource.Id.textView1);
            tw.Text = resp;
        }

        private void Parser_OnExceptionThrow(System.Exception ex)
        {
            Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();

            ;
        }

    }
}

