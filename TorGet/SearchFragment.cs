using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.Speech;
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;
using JoanZapata.XamarinIconify.Widget;
using Android.Views.Animations;

namespace TorGet
{

    public class SearchFragment : Android.Support.V4.App.Fragment
    {
        Android.Widget.SearchView searchView;
        private bool isRecording;
        private readonly int VOICE = 10;
        ImageButton speakButton;

        System.String[] categories = { "All", "Audio", "Video", "Applications", "Games", "Other" };
        System.String[] orderFilters = { "Default", "Name", "Date", "Size", "Seeders", "Leechers" };
        System.String[] providers = { "All", "ThePirateBay", "YIFY", "1337x", "RARBG", "KickassTorrents", "EZTV" };
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);



            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           
            isRecording = false;
            var view = inflater.Inflate(Resource.Layout.search_fragment, container, false);
            MainActivity.Instance.toolbar.SetTitle(Resource.String.search_title);
            MainActivity.Instance.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            //MainActivity.Instance.toolbar.Visibility = Android.Views.ViewStates.Gone;
            searchView = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchView);
            searchView.QueryTextSubmit += ShowSearchResults;

            

            
            var speakButton = view.FindViewById<ImageButton>(Resource.Id.ibSpeak);
            speakButton.SetImageDrawable(new IconDrawable(Application.Context, MaterialIcons.md_mic.ToString()).actionBarSize().colorRes(Resource.Color.colorTextWhite));
            speakButton.Click += MainActivity.Instance.ShowFilterDialog;
            
            //Clear searchview focus when background tapped
            var searchBG = view.FindViewById<LinearLayout>(Resource.Id.layoutSearchBG);
            searchBG.Click += (s, e) => { searchView.ClearFocus(); };

            //Category spinner setup
            var spinnerCategory = view.FindViewById<Spinner>(Resource.Id.spinnerCategory);
            spinnerCategory.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerCategory_ItemSelected);
            spinnerCategory.Touch += (s, e) => { searchView.ClearFocus(); spinnerCategory.PerformClick(); e.Handled = true; };
            spinnerCategory.Adapter = new CategoryListAdapter(MainActivity.Instance, categories);

           // var spinnerOrder = view.FindViewById<Spinner>(Resource.Id.spinnerOrder);
            //spinnerCategory.setOnItemSelectedListener(this);
            //spinnerOrder.Adapter = new OrderListAdapter(MainActivity.Instance, orderFilters);

            //Provider spinner setup
            var spinnerProvider = view.FindViewById<Spinner>(Resource.Id.spinnerProvider);
            //spinnerProvider.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerProvider_ItemSelected);
            spinnerProvider.Touch += (s, e) => { searchView.ClearFocus(); spinnerProvider.PerformClick(); e.Handled = true; };
            spinnerProvider.Adapter = new ProviderListAdapter(MainActivity.Instance, providers);

            var btnSearch = view.FindViewById<Button>(Resource.Id.btnSearch);
            btnSearch.Click += ShowSearchResults;

            

            return view;

            
        }

        private void spinnerCategory_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            
            Spinner spinner = (Spinner)sender;
            //spinner.RequestFocusFromTouch();
            var catId = spinner.GetItemAtPosition(e.Position).ToString();
            //Toast.MakeText(Application.Context, toast, ToastLength.Long).Show();
            switch (catId)
            {
                case"0":
                    MainActivity.Instance.searchCategory = TpbTorrentCategory.All;
                    break;

                case "1":
                    MainActivity.Instance.searchCategory = TpbTorrentCategory.AllAudio;
                    break;

                case "2":
                    MainActivity.Instance.searchCategory = TpbTorrentCategory.AllVideo;
                    break;

                case "3":
                    MainActivity.Instance.searchCategory = TpbTorrentCategory.AllApplication;
                    break;

                case "4":
                    MainActivity.Instance.searchCategory = TpbTorrentCategory.AllGames;
                    break;

                case "5":
                    MainActivity.Instance.searchCategory = TpbTorrentCategory.AllOther;
                    break;
            }
        }





        public void ShowSearchResults(System.Object o, EventArgs e)
        {
            searchView.ClearFocus();
            if (MainActivity.Instance.IsConnected == true)
            {
                SearchResultsFragment searchResultsFragment = new SearchResultsFragment();
                Bundle args = new Bundle();
                args.PutString("searchQuery", searchView.Query);
                searchResultsFragment.Arguments = args;
                MainActivity.Instance.SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.content_frame, searchResultsFragment)
                    .SetTransition(Android.Support.V4.App.FragmentTransaction.TransitFragmentFade)
                    .AddToBackStack(null)
                    .Commit();
            }
            if (MainActivity.Instance.IsConnected == false)
            {
                Toast.MakeText(Application.Context, "No internet connection available. Please check your connection and try again.", ToastLength.Short).Show();

            }
        }
    }
}