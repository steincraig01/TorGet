using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TorGet
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public JoanZapata.XamarinIconify.Widget.IconTextView Size { get; set; }
        public JoanZapata.XamarinIconify.Widget.IconTextView Uploaded { get; set; }
        public TextView Uploader { get; set; }
        public JoanZapata.XamarinIconify.Widget.IconTextView Seeds { get; set; }
        public JoanZapata.XamarinIconify.Widget.IconTextView Leechers { get; set; }
        public JoanZapata.XamarinIconify.Widget.IconTextView Category { get; set; }
        public JoanZapata.XamarinIconify.Widget.IconTextView Trusted { get; set; }
    }
}