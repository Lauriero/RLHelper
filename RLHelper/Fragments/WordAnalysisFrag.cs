using System;
using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace RLHelper.Fragments
{
    public class WordAnalysisFrag : Android.Support.V4.App.Fragment
    {
        View view;

        FrameLayout bottomSheetFragment;

        PageReceiver reciever;
        PageParser parser;

        string handleWord = "";
        string selectedText = "";

        bool isOpen = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.WordAnalysisFrag, container, false);

            bottomSheetFragment = view.FindViewById<FrameLayout>(Resource.Id.bottomsheetfragContent);

            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Add(bottomSheetFragment.Id, new BottomSheetFrag(), "BottomSheet Fragment");
            trans.Commit();

            Toast.MakeText(Context, "work", ToastLength.Long);

            reciever = new PageReceiver();
            reciever.OnPageParse += Reciever_OnPageRecieve;
            reciever.OnExceptionThrow += Reciever_OnExceptionThrow;

            parser = new PageParser();
            parser.OnNewMorphemeData += Parser_OnNewData;
            parser.OnNewSpellData += Parser_OnNewSpellData;

            Button handleButton = view.FindViewById<Button>(Resource.Id.handleBtn);
            handleButton.Click += HandleButton_Click;

            return view;
        }

        private void Parser_OnNewSpellData(Spell sp)
        { 
            colorText(view.FindViewById<TextView>(Resource.Id.generalSpellView), sp.word, sp.spellsPos);

            Paint pt = new Paint();
            pt.SetARGB(255, 0, 255, 0);

            string prefix = "пре";

            LinearLayout ll = view.FindViewById<LinearLayout>(Resource.Id.canvasLayout);
            TextView tw = view.FindViewById<TextView>(Resource.Id.generalSpellView);


            Bitmap bt = Bitmap.CreateBitmap(ll.Width, ll.Height, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(bt);
            canvas.DrawRect(new Rect((int)GetScreenX<TextView>(tw) - 15, 35, (int)GetScreenX<TextView>(tw) - 15 + prefix.Length * tw.Width / tw.Text.Length, 40), pt);
            canvas.DrawRect(new Rect((int)GetScreenX<TextView>(tw) - 15 + prefix.Length * tw.Width / tw.Text.Length - 5, 35, (int)GetScreenX<TextView>(tw) - 15 + prefix.Length * tw.Width / tw.Text.Length, 47), pt);

            ll.SetBackgroundDrawable(new BitmapDrawable(bt));

        }

        private void colorText(TextView tw, string text, List<int> colorPos)
        {
            SpannableStringBuilder t = new SpannableStringBuilder(text);
            ForegroundColorSpan style = new ForegroundColorSpan(Color.Rgb(255, 0, 0));

            foreach (int pos in colorPos) {
                t.SetSpan(style, pos, pos + 1, SpanTypes.Composing);
            }

            tw.TextFormatted = t;
        }

        private void Parser_OnNewData(MorphemeData data)
        {
            TextView tw = view.FindViewById<TextView>(Resource.Id.generalSpellView);
            tw.Text = data.prefixFirst + '\n' + data.root + '\n' + data.suffixFirst + '\n' + data.ending;
        }

        private async void HandleButton_Click(object sender, EventArgs e)
        {

            //LinearLayout lr = view.FindViewById<LinearLayout>(Resource.Id.spellsLayout);
            //lr.RemoveAllViews();

            EditText textInput = view.FindViewById<EditText>(Resource.Id.inputText);
            string text = textInput.Text;
            handleWord = text;

            await reciever.createHttpPostRequestAsync(text);


            if (isOpen) {
                var interpolator = new Android.Views.Animations.OvershootInterpolator(5);
                bottomSheetFragment.Animate().SetInterpolator(interpolator)
                                    .TranslationYBy(-310)
                                    .SetDuration(500);

                isOpen = false;
            }
            
        }

        public static double GetScreenX<TRenderer>(TRenderer renderer) where TRenderer : View
        {
            double screenCoordinateX = renderer.GetX();
            IViewParent viewParent = renderer.Parent;
            while (viewParent != null)
            {
                if (viewParent is ViewGroup group)
                {
                    screenCoordinateX += group.GetX();
                    viewParent = group.Parent;
                }
                else
                {
                    viewParent = null;
                }
            }
            return screenCoordinateX;
        }

        private async void Reciever_OnPageRecieve(string resp)
        {
            DateTime dt = DateTime.Now;
            await parser.SpellingParse(resp, handleWord);
            Toast.MakeText(Context, (dt - DateTime.Now).ToString(), ToastLength.Short).Show();
        }

        private void Reciever_OnExceptionThrow(Exception ex)
        {
            Toast.MakeText(Context, ex.ToString(), ToastLength.Short).Show();
        }
    }
}