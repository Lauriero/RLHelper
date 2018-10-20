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
    class MorphemeData
    {
        public string prefixFirst { get; set; }
        public string prefixSecond { get; set; }
        public string root { get; set; }
        public string suffixFirst { get; set; }
        public string suffixSecond { get; set; }
        public string basis { get; set; }
        public string ending { get; set; }
        public string postfix { get; set; }

    }
}