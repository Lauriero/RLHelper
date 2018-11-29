using Android.Content;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace RLHelper.Morphemes
{
    class Ending : Morpheme
    {

        public Ending(string s, Color c) : base(s, c) { }

        public override void Drow()
        {
            _canvas.DrawRect(new Rect(0, 7, _textWidth, 9), _pt);
            _canvas.DrawRect(new Rect(0, 7, 3, _bCLayout.Height - 1), _pt);
            _canvas.DrawRect(new Rect(_textWidth - 3, 7, _textWidth, _bCLayout.Height - 1), _pt);
            _canvas.DrawRect(new Rect(0, _bCLayout.Height - 3, _textWidth, _bCLayout.Height - 1), _pt);
        }

        public override void View()
        {
            _newTextView.SetBackgroundDrawable(new BitmapDrawable(_bt));
            _oBCLayout.AddView(_newTextView);
        }
    }
}