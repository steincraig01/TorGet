using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using static Android.App.ActionBar;
using Android.Views.Animations;
using Android.Support.Design.Widget;

namespace TorGet
{


    public class TorDetailDialogFragment : BottomSheetDialogFragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetStyle(StyleNormal, Resource.Style.Theme_Design_Light_BottomSheetDialog);
            

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            //String title = args.getString("title");

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.TorrentBottomSheetLayout, container, false);
            view.FindViewById<TextView>(Resource.Id.tvtorname).Text = Arguments.GetString("torname");
            //this.Dialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.FillParent);

            return view;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Dialog dialog = base.OnCreateDialog(savedInstanceState);
            //dialog.Window.Attributes.WindowAnimations = Resource.Animation.abc_grow_fade_in_from_bottom;


            return dialog;
            //return base.OnCreateDialog(savedInstanceState);
        }
    }
}