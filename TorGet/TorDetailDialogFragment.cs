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
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;
using JoanZapata.XamarinIconify.Widget;
using Xamarin.Essentials;
using TorrentTitleParser;


namespace TorGet
{


    public class TorDetailDialogFragment : BottomSheetDialogFragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNormal, Resource.Style.Theme_Design_Light_BottomSheetDialog);
            Iconify.with(new MaterialModule());
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.TorrentBottomSheetLayout, container, false);
            view.FindViewById<TextView>(Resource.Id.tvtorname).Text = Arguments.GetString("torname");
            LinearLayout open = view.FindViewById<LinearLayout>(Resource.Id.download);
            LinearLayout copy = view.FindViewById<LinearLayout>(Resource.Id.copymagnet);
            LinearLayout share = view.FindViewById<LinearLayout>(Resource.Id.sharemagnet);
            LinearLayout openinbrowser = view.FindViewById<LinearLayout>(Resource.Id.openinbrowser);
            LinearLayout bookmark = view.FindViewById<LinearLayout>(Resource.Id.bookmark);
            open.Click += openClick;
            copy.Click += copyClick;
            share.Click += shareClick;
            openinbrowser.Click += openInBrowserClick;
            bookmark.Click += bookmarkClick;
            return view;
        }

        public string ParseFileName(string fname)
        {
            var torname = new TorrentTitleParser.Torrent(fname);
            return torname.Title;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Dialog dialog = base.OnCreateDialog(savedInstanceState);
            return dialog;
            //return base.OnCreateDialog(savedInstanceState);
        }

        public void openClick(object o, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse(Arguments.GetString("magnet"));
            Intent torrentIntent = new Intent(Intent.ActionView, uri);
            try
            {
                StartActivity(torrentIntent);
            }
            catch (ActivityNotFoundException)
            {
                Android.Widget.Toast.MakeText(Application.Context, "No Torrent Client Found!", Android.Widget.ToastLength.Short).Show();
            }
            Dialog.Dismiss();
            //Android.Widget.Toast.MakeText(Application.Context, Arguments.GetString("torname"), Android.Widget.ToastLength.Short).Show();
        }

        public void copyClick(object o, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse(Arguments.GetString("magnet"));
            Clipboard.SetTextAsync(uri.ToString());
            Android.Widget.Toast.MakeText(Application.Context, "Magnet link copied to clipboard.", Android.Widget.ToastLength.Short).Show();
            Dialog.Dismiss();
        }

        public void shareClick(object o, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse(Arguments.GetString("magnet"));
            Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri.ToString(),
                Title = "Torrent Link"
            }) ;
            Dialog.Dismiss();
        }

        public void openInBrowserClick(object o, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse(Arguments.GetString("pageurl"));
            Intent torrentIntent = new Intent(Intent.ActionView, uri);
            StartActivity(torrentIntent);
            Dialog.Dismiss();
            //Android.Widget.Toast.MakeText(Application.Context, Arguments.GetString("torname"), Android.Widget.ToastLength.Short).Show();
        }

        public void bookmarkClick(object o, EventArgs e)
        {
            var fname = ParseFileName(Arguments.GetString("torname"));
            Dialog.Dismiss();
            Android.Widget.Toast.MakeText(Application.Context, fname, Android.Widget.ToastLength.Long).Show();
        }
    }
}