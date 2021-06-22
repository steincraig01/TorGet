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
        System.String[] providers = { "All", "TPB", "YTS", "RARBG", "KAT" };
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);



            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            isRecording = false;
            var view = inflater.Inflate(Resource.Layout.search_fragment, container, false);
            var searchBG = view.FindViewById<LinearLayout>(Resource.Id.layoutSearchBG);
            MainActivity.Instance.toolbar.SetTitle(Resource.String.search_title);
            MainActivity.Instance.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            MainActivity.Instance.toolbar.Visibility = Android.Views.ViewStates.Gone;
            searchView = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchView);
            var speakButton = view.FindViewById<ImageButton>(Resource.Id.ibSpeak);
            speakButton.SetImageDrawable(new IconDrawable(Application.Context, MaterialIcons.md_mic.ToString()).actionBarSize().colorRes(Resource.Color.colorTextWhite));
            speakButton.Click += MainActivity.Instance.ShowFilterDialog;




            var spinnerCategory = view.FindViewById<Spinner>(Resource.Id.spinnerCategory);
            //spinnerCategory.setOnItemSelectedListener(this);
            spinnerCategory.Adapter = new CategoryListAdapter(MainActivity.Instance, categories);

            var spinnerOrder = view.FindViewById<Spinner>(Resource.Id.spinnerOrder);
            //spinnerCategory.setOnItemSelectedListener(this);
            spinnerOrder.Adapter = new OrderListAdapter(MainActivity.Instance, orderFilters);

            var spinnerProvider = view.FindViewById<Spinner>(Resource.Id.spinnerProvider);
            //spinnerCategory.setOnItemSelectedListener(this);
            spinnerProvider.Adapter = new ProviderListAdapter(MainActivity.Instance, providers);


            //searchView = view.FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.searchView);
            //var simgid = searchView.Context.Resources.GetIdentifier("search_mag_icon", "id", );
            //var id = searchView.Context.Resources.GetIdentifier("search_src_text", "id", null);
            //var searchEditText = searchView.FindViewById<EditText>(id);
            //var searchIcon = searchView.FindViewById<ImageView>(simgid);
            //searchIcon.SetColorFilter(Android.Graphics.Color.White);
            //searchEditText.SetTextColor(Android.Graphics.Color.White);
            //searchEditText.SetHintTextColor(Android.Graphics.Color.LightGray);
            //searchView.SubmitButtonEnabled = true;
            var btnSearch = view.FindViewById<Button>(Resource.Id.btnSearch);
            btnSearch.Click += ShowSearchResults;

            

            return view;

            
        }





        public void ShowSearchResults(System.Object o, EventArgs e)
        {
            SearchResultsFragment searchResultsFragment = new SearchResultsFragment();
            Bundle args = new Bundle();
            args.PutString("searchQuery", searchView.Query);
            searchResultsFragment.Arguments = args;
            MainActivity.Instance.SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, searchResultsFragment)
                .AddToBackStack(null)
                .Commit();
        }
    }
}