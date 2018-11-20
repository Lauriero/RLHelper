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

        #region Свойства

        View view;

        FrameLayout bottomSheetFragment;

        PageReceiver reciever;
        PageParser parser;

        string handleWord = "";
        string selectedText = "";

        bool isOpen = true;

        #endregion

        #region Методы для отображения фрагмента и реализация при запуске

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
            reciever.OnMorphemePageParse += Reciever_OnMorphemePageRecieve;
            reciever.OnSpellsPageParse += Reciever_OnSpellsPageRecieve;
            reciever.OnExceptionThrow += Reciever_OnExceptionThrow;

            parser = new PageParser();
            parser.OnNewMorphemeData += Parser_OnNewMorphemeData;
            parser.OnNewSpellData += Parser_OnNewSpellData;
            parser.OnNewInformation += Parser_OnNewInformation;

            Button handleButton = view.FindViewById<Button>(Resource.Id.handleBtn);
            handleButton.Click += HandleButton_Click;

            return view;
        }

        #endregion

        #region Получение данных из парсера орфограмм и вывод их

        private void Parser_OnNewSpellData(Spell sp)
        { 
            colorText(view.FindViewById<TextView>(Resource.Id.generalSpellView), sp.word, sp.spellsPos);

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

        #endregion

        #region Получение данных из парсера морфем и вывод с графикой

        private void Parser_OnNewMorphemeData(MorphemeData data)
        {
            Toast.MakeText(Context, "woork", ToastLength.Long).Show();

            ViewMorphemes(data);

            
        }

        private void ViewMorphemes(MorphemeData data) {

            TextView tw = view.FindViewById<TextView>(Resource.Id.generalSpellView);
            tw.Text = "fwa";

            if (data.prefixFirst != "") {
                drowPrefix(data.prefixFirst);
                if (data.prefixSecond != "") {
                    drowPrefix(data.prefixSecond);
                }
            }

            drowRoot(data.root);

            if (data.suffixFirst != "") {
                drowSuffix(data.suffixFirst);
                if (data.suffixSecond != "") {
                    drowSuffix(data.suffixSecond);
                }
            }

            if (data.ending != "") {
                if (data.ending == "null") {
                    drowNullEnding();
                } else {
                    drowEnding(data.ending);
                }
                
            }
        }

        private void drowNullEnding()
        {
            ;
        }

        private void drowEnding(string ending)
        {
            ;
        }

        private void drowSuffix(string suffixFirst)
        {
            ;
        }

        private void drowRoot(string root)
        {
            ;
        }

        private void drowPrefix(string prefixFirst)
        {
            Toast.MakeText(Context, prefixFirst, ToastLength.Long).Show();

           

            LinearLayout ll = view.FindViewById<LinearLayout>(Resource.Id.linearLayout4);
            TextView newTextView = new TextView(Context);

            newTextView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            newTextView.SetTextAppearance(Resource.Style.morphemeTextAppearance);

            newTextView.Text = prefixFirst;

            Paint pt = new Paint();
            pt.SetARGB(255, 0, 255, 0);

            Paint mp = new Paint();
            mp.SetTypeface(newTextView.Typeface);
            mp.TextScaleX = newTextView.TextScaleX;
            mp.TextSize = newTextView.TextSize;
            mp.Color = Color.White;

            Bitmap bt = Bitmap.CreateBitmap((int)mp.MeasureText(prefixFirst), newTextView.Height, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(bt);

            canvas.DrawRect(new Rect(0, 5, (int)mp.MeasureText(prefixFirst), 10), pt);

            newTextView.SetBackgroundDrawable(new BitmapDrawable(bt));

            ll.AddView(newTextView);
        }

        #endregion

        #region Обработка взаимодействия с элементами управления

        private async void HandleButton_Click(object sender, EventArgs e)
        {

            EditText textInput = view.FindViewById<EditText>(Resource.Id.inputText);
            string text = textInput.Text;
            handleWord = text;

            //await reciever.createHttpPostRequestAsync(text);

            if (isOpen) {
                var interpolator = new Android.Views.Animations.OvershootInterpolator(5);
                bottomSheetFragment.Animate().SetInterpolator(interpolator)
                                    .TranslationYBy(-310)
                                    .SetDuration(500);

                isOpen = false;
            }

            await reciever.createHttpGetRequestAsync(text);

        }

        #endregion

        #region Отправка полученных данных парсерам

        private async void Reciever_OnSpellsPageRecieve(string resp)
        {
            DateTime dt = DateTime.Now;
            await parser.SpellingParse(resp, handleWord);
            Toast.MakeText(Context, (DateTime.Now - dt).ToString(), ToastLength.Short).Show();
        }

        private async void Reciever_OnMorphemePageRecieve(string resp)
        {
            DateTime dt = DateTime.Now;
            await parser.MorphemeParse(resp);
            TextView tw = view.FindViewById<TextView>(Resource.Id.generalSpellView);
            //tw.Text = "Обработка...";
            //Toast.MakeText(Context, (DateTime.Now - dt).ToString(), ToastLength.Short).Show();
        }

        #endregion

        #region Обработка исключений

        private void Reciever_OnExceptionThrow(Exception ex)
        {
            Toast.MakeText(Context, ex.ToString(), ToastLength.Short).Show();
        }

        private void Parser_OnNewInformation(string obj)
        {
            TextView tw = view.FindViewById<TextView>(Resource.Id.generalSpellView);
            tw.Text = obj;

            Toast.MakeText(Context, obj, ToastLength.Short).Show();
        }

        #endregion

    }
}