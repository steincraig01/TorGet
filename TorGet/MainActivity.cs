﻿using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using static Android.App.ActionBar;
using Xamarin.Essentials;
using Android.Animation;
using System.Collections.ObjectModel;
using Android.Graphics;
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;
using JoanZapata.XamarinIconify.Widget;
using Android.Views.Animations;
using System;
using Java.Lang;
using Android.Support.Design.Widget;
using System.Net;
using Android.Text;
using System.Threading.Tasks;

namespace TorGet
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppThemeDark", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static MainActivity Instance;
        public List<Torrent> torrents;
        ListView listView;
        Android.Support.V7.Widget.SearchView torSearchView;
        Android.Support.Design.Button.MaterialButton btnSaveFilters, btnCancelFilters;
        RadioGroup rgCategories, rgOrderBy;
        RadioButton rbSelectedCategory, rbSelectedOrderBy;
        public int selectedCatId, selectedOrderById;
        Dialog filterDialog;
        Dialog torrentDialog;
        RelativeLayout rlStatusLayout;
        public Android.Support.V7.Widget.Toolbar toolbar;
        IconTextView tvStatusText;
        RelativeLayout layoutWelcome;
        LinearLayout llMainActivity;
        Snackbar sbMainactivity;
        ImageButton btnClearSearch;
        Animation statusAnimShow;
        Animation listViewAnimShow;
        Animation statusAnimHide;
        Animation listViewAnimHide;
        Animation welcomeAnimShow;
        public int searchCategory;
        public TpbQueryOrder searchOrder;
        public bool IsConnected;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Instance = this;
            
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "TorrentTools";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_backarrow);
            llMainActivity = FindViewById<LinearLayout>(Resource.Id.llMainActivity);
            //Instantiate torrent list to ensure not null when using AddRange function
            torrents = new List<Torrent>();

            
            statusAnimShow = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_slide_in_top);
            listViewAnimShow = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_slide_in_bottom);
            statusAnimHide = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_fade_out);
            welcomeAnimShow = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_grow_fade_in_from_bottom);
            welcomeAnimShow.Duration = 1000;
           
            listViewAnimHide = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_slide_out_bottom);
            
            Iconify.with(new MaterialModule());
            Iconify.with(new FontAwesomeModule());
            Iconify.with(new MaterialCommunityModule());
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Task checkConnectionTask = new Task(() => CheckInternetConnection());
            checkConnectionTask.Start();
            IsConnected = true;
            searchCategory = TpbTorrentCategory.All;
            searchOrder = TpbQueryOrder.ByDefault;

            var searchFragment = new SearchFragment();
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, searchFragment)
                .Commit();

        }

        public void LoadFragment(string fragmentName)
        {
            Android.Support.V4.App.Fragment fragment = null;

            if (fragmentName == "SearchFragment")
                fragment = new SearchFragment();
            if (fragmentName == "SearchResultsFragment")
                fragment = new SearchResultsFragment();

            if (fragment == null)
                return;


            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .AddToBackStack(null)
                .Commit();
        }

        //protected override void OnStart()
        //{
            //Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            //layoutWelcome.Visibility = ViewStates.Visible;
            //CheckInternetConnection();
            //base.OnStart();
        //}

        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;
            if (access == Xamarin.Essentials.NetworkAccess.None)
                IsConnected = false;
            CheckInternetConnection();
            //Toast.MakeText(this, "No internet connection detected", ToastLength.Short).Show();

        }

        public void CheckInternetConnection()
        {
            string CheckUrl = "http://google.com";
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                iNetResponse.Close();
                IsConnected = true;
            }
            catch (WebException ex)
            {
                IsConnected = false;
                RunOnUiThread(() => Toast.MakeText(this, "No internet connection detected!", ToastLength.Long).Show());
                //Snackbar.Make(llMainActivity, "I'm a snackbar!", Snackbar.LengthIndefinite).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = torrents[e.Position];
            //Android.Widget.Toast.MakeText(this, t.Name, Android.Widget.ToastLength.Short).Show();
            Bundle mybundle = new Bundle();
            mybundle.PutString("torname", t.Name);
            mybundle.PutString("magnet", t.Magnet);
            mybundle.PutString("pageurl", t.PageUrl);
            //mybundle.PutString("torname", t.Name);
            TorDetailDialogFragment modalBottomSheet = new TorDetailDialogFragment();
            modalBottomSheet.Arguments = mybundle;
            modalBottomSheet.Show(SupportFragmentManager, "modalMenu");


            //View dialogView = LayoutInflater.Inflate(Resource.Layout.TorrentDetailDialog, null);
            //BottomSheetDialog dialog = new BottomSheetDialog(this);
            //dialog.SetContentView(dialogView);
            //dialog.Show();

            //FragmentTransaction transaction = FragmentManager.BeginTransaction().SetCustomAnimations(Resource.Animation.abc_grow_fade_in_from_bottom, Resource.Animation.abc_shrink_fade_out_from_bottom) ;
            //TorDetailDialogFragment detdialog = new TorDetailDialogFragment();
            //detdialog.Show(transaction, "Dialog Fragment"); 

            //torrentDialog = new Dialog(this);
            //torrentDialog.SetContentView(Resource.Layout.TorrentDetailDialog);
            //torrentDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);

            torSearchView.ClearFocus();
            torSearchView.SetQuery("", false);

            //MenuItemCompat.CollapseActionView(item);
            //torrentDialog.Show();

            // detailDialogToolbar = torrentDialog.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.tordetaildialog_toolbar);
            //detailDialogToolbar.SetNavigationIcon(Resource.Drawable.ic_backarrow);
            //detailDialogToolbar.SetOnMenuItemClickListener(new Android.Support.V7.Widget.Toolbar.on() {

            //torrentDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.FillParent);
            //torrentDialog.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);;

            //tvTorName = torrentDialog.FindViewById<TextView>(Resource.Id.tvTorrentName);
            //tvTorUploaded = torrentDialog.FindViewById<TextView>(Resource.Id.tvTorrentUploaded);
            //tvTorName.Text = t.Name;
            //tvTorUploaded.Text = t.Uploaded;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            MenuInflater.Inflate(Resource.Menu.toolmenu, menu);
            

            return base.OnCreateOptionsMenu(menu);
        }

        public void ShowFilterDialog(System.Object o, EventArgs e)
        {
            filterDialog = new Dialog(this);
            filterDialog.SetContentView(Resource.Layout.sort_filter_layout);
            filterDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
            //searchDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);

            filterDialog.Show();
            filterDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.FillParent);
            filterDialog.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
            btnSaveFilters = filterDialog.FindViewById<Android.Support.Design.Button.MaterialButton>(Resource.Id.btnSaveFilters);
            btnSaveFilters.Click += BtnSaveFilters_Click;
            btnCancelFilters = filterDialog.FindViewById<Android.Support.Design.Button.MaterialButton>(Resource.Id.btnCancelFilters);
            btnCancelFilters.Click += (s, e) => { filterDialog.Dismiss(); };
            rgCategories = filterDialog.FindViewById<RadioGroup>(Resource.Id.rgCategory);
            rgOrderBy = filterDialog.FindViewById<RadioGroup>(Resource.Id.rgOrderBy);
            //Toast.MakeText(this, selectedCatId.ToString(), ToastLength.Short).Show();
            if (selectedCatId != 0)
                rgCategories.Check(selectedCatId);
            if (selectedOrderById != 0)
                rgOrderBy.Check(selectedOrderById);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToString() == "Filter")
            {
                filterDialog = new Dialog(this);
                filterDialog.SetContentView(Resource.Layout.sort_filter_layout);
                filterDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
                //searchDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);

                filterDialog.Show();
                filterDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.WrapContent);
                filterDialog.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
                btnSaveFilters = filterDialog.FindViewById<Android.Support.Design.Button.MaterialButton>(Resource.Id.btnSaveFilters);
                btnSaveFilters.Click += BtnSaveFilters_Click;
                btnCancelFilters = filterDialog.FindViewById<Android.Support.Design.Button.MaterialButton>(Resource.Id.btnCancelFilters);
                btnCancelFilters.Click += (s, e) => { filterDialog.Dismiss(); };
                rgCategories = filterDialog.FindViewById<RadioGroup>(Resource.Id.rgCategory);
                rgOrderBy = filterDialog.FindViewById<RadioGroup>(Resource.Id.rgOrderBy);
                //Toast.MakeText(this, selectedCatId.ToString(), ToastLength.Short).Show();
                if (selectedCatId != 0)
                    rgCategories.Check(selectedCatId);
                if (selectedOrderById != 0)
                    rgOrderBy.Check(selectedOrderById);


            }
 
            if (item.ItemId == Android.Resource.Id.Home)
            {
                this.OnBackPressed();
            }
            //Toast.MakeText(this, item.ItemId.ToString(), ToastLength.Short).Show();
            //Toast.MakeText(this, "Action selected: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
        
        private void BtnSaveFilters_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "Search Filters Saved", ToastLength.Short).Show();
            
            selectedCatId = rgCategories.CheckedRadioButtonId;
            selectedOrderById = rgOrderBy.CheckedRadioButtonId;
            rbSelectedCategory = filterDialog.FindViewById<RadioButton>(selectedCatId);
            rbSelectedOrderBy = filterDialog.FindViewById<RadioButton>(selectedOrderById);
            switch (rbSelectedCategory.Text)
            {
                case "All":
                    searchCategory = TpbTorrentCategory.All;
                    break;

                case "Audio":
                    searchCategory = TpbTorrentCategory.AllAudio;
                    break;

                case "Video":
                    searchCategory = TpbTorrentCategory.AllVideo;
                    break;

                case "Applications":
                    searchCategory = TpbTorrentCategory.AllApplication;
                    break;

                case "Games":
                    searchCategory = TpbTorrentCategory.AllGames;
                    break;

                case "Porn":
                    searchCategory = TpbTorrentCategory.AllPorn;
                    break;

                case "Other":
                    searchCategory = TpbTorrentCategory.AllOther;
                    break;
            }

            switch (rbSelectedOrderBy.Text)
            {
                case "Default":
                    searchOrder = TpbQueryOrder.ByDefault;
                    break;

                case "Name":
                    searchOrder = TpbQueryOrder.ByName;
                    break;

                case "Date":
                    searchOrder = TpbQueryOrder.ByUploaded;
                    break;

                case "Size":
                    searchOrder = TpbQueryOrder.BySize;
                    break;

                case "Seeds":
                    searchOrder = TpbQueryOrder.BySeeds;
                    break;

                case "Leechers":
                    searchOrder = TpbQueryOrder.ByLeechers;
                    break;

                case "Uploader":
                    searchOrder = TpbQueryOrder.ByUledBy;
                    break;
            }
            filterDialog.Dismiss();
        }
        
    }
   
}
