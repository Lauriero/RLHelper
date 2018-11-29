using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;

namespace RLHelper.Morphemes
{
    class Prefix : Morpheme
    {

        public Prefix(string s, Color c) : base(s, c) { }

        public override void Drow()
        { 
            _canvas.DrawRect(new Rect(_morphPadding, 7, _textWidth - _morphPadding + 1, 9), _pt);
            _canvas.DrawRect(new Rect(_textWidth - _morphPadding - 2, 7, _textWidth - _morphPadding + 1, 11), _pt);
        }
    }
}