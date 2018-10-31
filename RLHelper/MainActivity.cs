using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using RLHelper.Fragments;

namespace RLHelper
{
    [Activity(Label = "RLHelper", MainLauncher = true, Icon = "@drawable/icon", Theme = "Theme.AppCompat")]
    public class MainActivity : AppCompatActivity
    {
        public FrameLayout bottomSheetFragContent;
       

        PageReceiver reciever;
        PageParser parser;

        string handleWord = "";
        string selectedText = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.currentlayoutContent, new WordAnalysisFrag(), "AnalysisFragment");
            trans.Commit();

            

           
            //reciever = new PageReceiver();
            //reciever.OnPageParse += Parser_OnPageParser;
            //reciever.OnExceptionThrow += Parser_OnExceptionThrow;

            //parser = new PageParser();
            //parser.OnNewMorphemeData += Parser_OnNewData;
            //parser.OnNewSpellData += Parser_OnNewSpellData;


            //Button handleButton = FindViewById<Button>(Resource.Id.button1);

            //handleButton.Click += HandleButton_Click;
        }

        //private void Parser_OnNewSpellData(List<Spell> spells)
        //{
        //    foreach (Spell sp in spells) {
                
        //        TextView newTextView = new TextView(this);

        //        newTextView.SetTextColor(Color.Rgb(255, 255, 255));
        //        newTextView.Gravity = GravityFlags.CenterHorizontal;
        //        LinearLayout.LayoutParams par = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

        //        if (spells.IndexOf(sp) == 0) {
        //            selectedText = sp.word;
        //            par.LeftMargin = 50;
        //            newTextView.TextSize = 25;
        //            newTextView.Id = Resource.Id.generalSpellView;
        //        } else {
        //            par.LeftMargin = 90;
        //            newTextView.TextSize = 15;
        //            newTextView.Click += NewTextView_Click;
        //        }

        //        newTextView.LayoutParameters = par;

        //        LinearLayout lr = FindViewById<LinearLayout>(Resource.Id.spellsLayout);
        //        lr.AddView(newTextView);

        //        colorText(newTextView, sp.word, sp.spellsPos);
        //    }
        //}

        //private void NewTextView_Click(object sender, EventArgs e)
        //{
        //    TextView view = sender as TextView;
        //    TextView gView = FindViewById<TextView>(Resource.Id.generalSpellView);

        //    string temp = gView.Text;

        //    gView.Text = view.Text;
        //    view.Text = temp;

        //    selectedText = view.Text;
        //}

        //private void colorText(TextView tw, string text, List<int> colorPos) {
        //    SpannableStringBuilder t = new SpannableStringBuilder(text);
        //    ForegroundColorSpan style = new ForegroundColorSpan(Color.Rgb(255, 0, 0));
            
        //    foreach (int pos in colorPos) {
        //        t.SetSpan(style, pos, pos + 1, SpanTypes.Composing);
        //    }

        //    tw.TextFormatted = t;
        //}

        //private void Parser_OnNewData(MorphemeData data)
        //{
        //    TextView tw = FindViewById<TextView>(Resource.Id.generalSpellView);
        //    tw.Text = data.prefixFirst + '\n' + data.root + '\n' + data.suffixFirst + '\n' + data.ending;
        //}

        //private async void HandleButton_Click(object sender, EventArgs e)
        //{
        //    LinearLayout lr = FindViewById<LinearLayout>(Resource.Id.spellsLayout);
        //    lr.RemoveAllViews();

        //    EditText textInput = FindViewById<EditText>(Resource.Id.editText1);
        //    string text = textInput.Text;
        //    handleWord = text;

        //    await reciever.createHttpPostRequestAsync(text);
        //}

        //private async void Parser_OnPageParser(string resp)
        //{
        //    DateTime dt = DateTime.Now;
        //    await parser.SpellingParse(resp, handleWord);
        //    Toast.MakeText(this, (dt - DateTime.Now).ToString(), ToastLength.Short).Show();
        //}

        //private void Parser_OnExceptionThrow(Exception ex)
        //{
        //    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
        //}

    }
}

