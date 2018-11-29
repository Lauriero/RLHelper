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
    class Root : Morpheme
    {
        public Root(string s, Color c) : base(s, c) { }

        public override void Drow()
        {
            //_pt.SetStyle(Paint.Style.Stroke);
            //_pt.StrokeWidth = 2;

            //RectF oval = new RectF();
            //oval.Set(_morphPadding, 7, _textWidth - _morphPadding, 14);

            //_canvas.DrawArc(oval, 70, 290, false, _pt);       
        }
    }
}