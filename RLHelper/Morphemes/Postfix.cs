using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace RLHelper.Morphemes
{
    class Postfix : Morpheme
    {
        public Postfix(string s, Color c) : base(s, c) { }

        public override void Drow()
        {
            _canvas.DrawRect(new Rect(2, 7, 5, 11), _pt);
            _canvas.DrawRect(new Rect(2, 7, _textWidth - _morphPadding, 9), _pt);  
        }

        public override void View()
        {
            _newTextView.SetBackgroundDrawable(new BitmapDrawable(_bt));
            _oBCLayout.AddView(_newTextView);
        }
    }
}