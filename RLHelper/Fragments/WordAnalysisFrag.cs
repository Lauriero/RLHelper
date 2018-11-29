using System;
using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views.Animations;
using RLHelper.Morphemes;

namespace RLHelper.Fragments
{
    public class WordAnalysisFrag : Android.Support.V4.App.Fragment, View.IOnTouchListener
    {

        #region Свойства

        View view;

        FrameLayout bottomSheetFragment;

        PageReceiver reciever;
        PageParser parser;

        string handleWord = "";
        string selectedText = "";

        float mLastPosY;
        float normalShootTrans;
        float transDown = 0;

        bool isOpen = false;
        bool isFullOpen = false;

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
            //parser.OnNewSpellData += Parser_OnNewSpellData;
            parser.OnNewInformation += Parser_OnNewInformation;

            Button handleButton = view.FindViewById<Button>(Resource.Id.handleBtn);
            handleButton.Click += HandleButton_Click;

            return view;
        }

        #endregion

        #region Получение данных из парсера орфограмм и вывод их

        //private void Parser_OnNewSpellData(Spell sp)
        //{ 
        //    colorText(view.FindViewById<TextView>(Resource.Id.generalSpellView), sp.word, sp.spellsPos);

        //}

        //private void colorText(TextView tw, string text, List<int> colorPos)
        //{
        //    SpannableStringBuilder t = new SpannableStringBuilder(text);
        //    ForegroundColorSpan style = new ForegroundColorSpan(Color.Rgb(255, 0, 0));

        //    foreach (int pos in colorPos) {
        //        t.SetSpan(style, pos, pos + 1, SpanTypes.Composing);
        //    }

        //    tw.TextFormatted = t;
        //}

        #endregion

        #region Получение данных из парсера морфем и вывод с графикой

        private void Parser_OnNewMorphemeData(List<Morpheme> mList)
        {
            LinearLayout bLayout = view.FindViewById<LinearLayout>(Resource.Id.WorldBaseContainerLayout);
            LinearLayout oBLayout = view.FindViewById<LinearLayout>(Resource.Id.OutBaseContainerLayout);

            foreach (Morpheme morph in mList) {
                
                morph.InitializeDrowing(Context, bLayout, oBLayout);
                morph.Drow();
                morph.View();
            }
        }

        
        #endregion

        #region Обработка взаимодействия с элементами управления

        private async void HandleButton_Click(object sender, EventArgs e)
        {

            EditText textInput = view.FindViewById<EditText>(Resource.Id.inputText);
            string text = textInput.Text;
            handleWord = text;

            //await reciever.createHttpPostRequestAsync(text);

            LinearLayout bLayout = view.FindViewById<LinearLayout>(Resource.Id.WorldBaseContainerLayout);
            LinearLayout oBLayout = view.FindViewById<LinearLayout>(Resource.Id.OutBaseContainerLayout);
            bLayout.RemoveAllViews();
            oBLayout.RemoveAllViews();

            if (!isOpen) {

                float fTranslation = bottomSheetFragment.TranslationY;

                var interpolator = new OvershootInterpolator(5);
                bottomSheetFragment.Animate().SetInterpolator(interpolator)
                                    .TranslationYBy(-310)
                                    .SetDuration(500);

                isOpen = true;

                normalShootTrans = fTranslation - 310;
                Toast.MakeText(Context, normalShootTrans.ToString(), ToastLength.Short).Show();

                bottomSheetFragment.SetOnTouchListener(this);
            }

            await reciever.createHttpGetRequestAsync(text);

        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch(e.Action) {
                case MotionEventActions.Down:
                    mLastPosY = e.GetY();
                    transDown = v.TranslationY;
                    return true;
                case MotionEventActions.Move:
                    float currentPos = e.GetY();
                    float deltaY = mLastPosY - currentPos;

                    var transY = v.TranslationY;
                    transY -= deltaY;

                    if (transY < 0) {
                        transY = 0;
                    }
                                            
                    if (transY > normalShootTrans) {
                        transY = normalShootTrans;
                    }

                    v.TranslationY = transY;

                    return true;
                case MotionEventActions.Up:
                    float deltaTrans = v.TranslationY - transDown;

                    if (deltaTrans < -60) {
                        sheetUp();
                    } else if (deltaTrans > -60 && deltaTrans < 0) {
                        sheetDefault();
                    } else if (deltaTrans > 0) {
                        sheetDown();
                    }

                    return true;
                default:
                    return v.OnTouchEvent(e);
                    
            }
        }

        private void sheetUp() {
            bottomSheetFragment.Animate().SetInterpolator(new AccelerateDecelerateInterpolator()).TranslationY(0).SetDuration(600);
            isFullOpen = true;
        }

        private void sheetDown()
        {
            bottomSheetFragment.Animate().SetInterpolator(new AccelerateDecelerateInterpolator()).TranslationY(normalShootTrans).SetDuration(600);
            isFullOpen = false;
        }

        private void sheetDefault() {
            bottomSheetFragment.Animate().SetInterpolator(new AccelerateDecelerateInterpolator()).TranslationY(normalShootTrans).SetDuration(300);
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
            Toast.MakeText(Context, (DateTime.Now - dt).ToString(), ToastLength.Short).Show();
        }

        #endregion

        #region Обработка исключений

        private void Reciever_OnExceptionThrow(Exception ex)
        {
            Toast.MakeText(Context, ex.ToString(), ToastLength.Short).Show();
        }

        private void Parser_OnNewInformation(string obj)
        {

            Toast.MakeText(Context, obj, ToastLength.Short).Show();
        }

        #endregion


    }
}