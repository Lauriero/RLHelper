using Android.Content;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using Android.Graphics.Drawables;
using System;
using Android.Util;

namespace RLHelper.Morphemes
{
    class Morpheme
    {
        public string morphemeText { get; set; }
        public TextView newTextView { get; set; }


        protected Paint _pt;
        protected Canvas _canvas;
        protected Context _cont;
        protected LinearLayout _bCLayout;
        protected LinearLayout _oBCLayout;
        protected int _textWidth;
        protected int _morphPadding;
        protected Bitmap _bt;


        public Morpheme(string mText, Color c) {
            morphemeText = mText;

            _pt = new Paint();
            _pt.SetARGB(255, c.R, c.G, c.B);
        }

        public void InitializeDrowing(Context c, LinearLayout baseContainer, LinearLayout outbaseContainer) {
            _bCLayout = baseContainer;
            _oBCLayout = outbaseContainer;
            _cont = c;

            _morphPadding = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 3, _cont.Resources.DisplayMetrics);

            newTextView = new TextView(c);

            newTextView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            newTextView.SetPadding(_morphPadding, 2, _morphPadding, 0);
            newTextView.SetTextAppearance(_cont, Resource.Style.morphemeTextAppearance);

            newTextView.Text = morphemeText;

            Paint mp = new Paint();
            mp.SetTypeface(newTextView.Typeface);
            mp.TextScaleX = newTextView.TextScaleX;
            mp.TextSize = newTextView.TextSize;

            _textWidth = (int)mp.MeasureText(morphemeText);

            _bt = Bitmap.CreateBitmap(_textWidth, _bCLayout.Height, Bitmap.Config.Argb8888);

            _canvas = new Canvas(_bt);
        }

        public virtual void Drow() { ; }

        public virtual void View() {
            newTextView.SetBackgroundDrawable(new BitmapDrawable(_bt));
            _bCLayout.AddView(newTextView);
        }

    }
}