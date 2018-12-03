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
using Android.Util;
using System.IO;

namespace RLHelper.Fragments
{
    public class WordAnalysisFrag : Android.Support.V4.App.Fragment, View.IOnTouchListener
    {

        #region Свойства

        View view;

        FrameLayout bottomSheetFragment;

        PageReceiver reciever;
        PageParser parser;

        Spell wordSpell;

        string handleWord = "";
        string selectedText = "";

        float mLastPosY;
        float transDown = 0;
        float semiOpenShootTrans;

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

            view.Post(() => {
                OnLoadView();
            });

            return view;
        }

        public void OnLoadView() {

            int fragmentHigh = View.Height;

            bottomSheetFragment = view.FindViewById<FrameLayout>(Resource.Id.bottomsheetfragContent);
            bottomSheetFragment.TranslationY = bottomSheetFragment.Height;
            semiOpenShootTrans = bottomSheetFragment.TranslationY - bottomSheetFragment.TranslationY / 4;

            Toast.MakeText(Context, bottomSheetFragment.Height.ToString(), ToastLength.Short).Show();

            var trans = Activity.SupportFragmentManager.BeginTransaction();
            trans.Add(bottomSheetFragment.Id, new BottomSheetFrag(), "BottomSheet Fragment");
            trans.Commit();

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

        }

        #endregion

        #region Получение данных из парсера орфограмм и вывод их

        private async void Parser_OnNewSpellData(Spell sp)
        {
            wordSpell = sp;

            await reciever.createHttpGetRequestAsync(sp.word);
        }

        private void colorText(TextView tw, string text, List<int> colorPos)
        {
            SpannableStringBuilder t = new SpannableStringBuilder(text);
            ForegroundColorSpan style = new ForegroundColorSpan(Color.Rgb(255, 0, 0));

            foreach (int pos in colorPos) {
                t.SetSpan(style, pos, pos + 1, SpanTypes.Composing);
                Toast.MakeText(Context, pos, ToastLength.Short).Show();
            }

            tw.TextFormatted = t;
        }

        #endregion

        #region Получение данных из парсера морфем и вывод с графикой

        private void Parser_OnNewMorphemeData(List<Morpheme> mList)
        {
            int strPos = 0;

            LinearLayout bLayout = view.FindViewById<LinearLayout>(Resource.Id.WorldBaseContainerLayout);
            LinearLayout oBLayout = view.FindViewById<LinearLayout>(Resource.Id.OutBaseContainerLayout);

            foreach (Morpheme morph in mList) {
                morph.InitializeDrowing(Context, bLayout, oBLayout);

                List<int> spellPositions = new List<int>();
                foreach (int spPos in wordSpell.spellsPos) {
                    if (spPos > strPos) {
                        if (spPos < strPos + morph.morphemeText.Length - 1) {
                            spellPositions.Add(spPos - strPos);
                        } else { break; }
                    }
                }
                if (spellPositions.Count > 0) { colorText(morph.newTextView, morph.morphemeText, spellPositions); }

                strPos += morph.morphemeText.Length;

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

            await reciever.createHttpPostRequestAsync(text);

            LinearLayout bLayout = view.FindViewById<LinearLayout>(Resource.Id.WorldBaseContainerLayout);
            LinearLayout oBLayout = view.FindViewById<LinearLayout>(Resource.Id.OutBaseContainerLayout);
            bLayout.RemoveAllViews();
            oBLayout.RemoveAllViews();

            if (!isOpen) {

                var interpolator = new OvershootInterpolator(5);
                bottomSheetFragment.Animate().SetInterpolator(interpolator)
                                    .TranslationY(semiOpenShootTrans)
                                    .SetDuration(500);

                isOpen = true;

                Toast.MakeText(Context, semiOpenShootTrans.ToString(), ToastLength.Short).Show();

                bottomSheetFragment.SetOnTouchListener(this);
            }

            

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
                                            
                    if (transY > semiOpenShootTrans) {
                        transY = semiOpenShootTrans;
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
            bottomSheetFragment.Animate().SetInterpolator(new AccelerateDecelerateInterpolator()).TranslationY(semiOpenShootTrans).SetDuration(600);
            isFullOpen = false;
        }

        private void sheetDefault() {
            bottomSheetFragment.Animate().SetInterpolator(new AccelerateDecelerateInterpolator()).TranslationY(semiOpenShootTrans).SetDuration(300);
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

        #region Дополнительные методы для перевода, исключений и т.д.

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