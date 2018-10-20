using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace RLHelper
{
    [Activity(Label = "RLHelper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        PageReceiver reciever;
        PageParser parser;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //reciever = new PageReceiver();
            //reciever.OnPageParse += Parser_OnPageParser;
            //reciever.OnExceptionThrow += Parser_OnExceptionThrow;

            //parser = new PageParser();
            //parser.OnNewData += Parser_OnNewData;


            //Button handleButton = FindViewById<Button>(Resource.Id.button1);

            //handleButton.Click += HandleButton_Click;


        }

        //private void Parser_OnNewData(MorphemeData data)
        //{
        //    TextView tw = FindViewById<TextView>(Resource.Id.textView1);
        //    tw.Text = data.prefixFirst + '\n' + data.root + '\n' + data.suffixFirst + '\n' + data.ending;
        //}

        //private void HandleButton_Click(object sender, System.EventArgs e)
        //{
        //    EditText textInput = FindViewById<EditText>(Resource.Id.editText1);
        //    string text = textInput.Text;


        //    reciever.Start(text);
        //}

        //private void Parser_OnPageParser(string resp)
        //{
        //    DateTime dt = DateTime.Now;
        //    parser.Parse(resp);
        //    Toast.MakeText(this, (dt - DateTime.Now).ToString(), ToastLength.Short).Show();
        //}

        //private void Parser_OnExceptionThrow(System.Exception ex)
        //{
        //    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
        //}

    }
}

