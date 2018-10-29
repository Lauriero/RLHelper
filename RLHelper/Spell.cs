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

namespace RLHelper
{
    class Spell
    {
        public string word { get; set; }
        public List<int> spellsPos { get; set; }
    }
}