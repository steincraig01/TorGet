using Acr.UserDialogs;
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

namespace TorGet
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        List<Torrent> torrents;
        ListView listView;
        Android.Support.V7.Widget.SearchView torSearchView;
        Button btnSearch;
        EditText edtSearchQuery;
        Dialog searchDialog;
        Dialog torrentDialog;
        Spinner spinnerCategory;

        TextView tvTorName;
        TextView tvTorUploaded;
        TextView tvTorUploader;
        TextView tvTorCategory;
        TextView tvTorSize;
        TextView tvTorSeedLeech;
        RelativeLayout rlStatusLayout;
        Android.Support.V7.Widget.Toolbar toolbar;
        IconTextView tvStatusText;
        Typeface customFont;
        RelativeLayout layoutWelcome;
        ImageButton btnClearSearch;
        Animation statusAnimShow;
        Animation listViewAnimShow;
        Animation statusAnimHide;
        Animation listViewAnimHide;
        Animation welcomeAnimShow;






        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            customFont = Typeface.CreateFromAsset(Application.Assets, "commonfont.ttf");
            listView = FindViewById<ListView>(Resource.Id.lvresults);
            listView.ItemClick += OnListItemClick;
            listView.SetPadding(10, 10, 10, 10);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "TorrentTools";
            //btnClearSearch.Click += BtnClearSearch_Click;
            layoutWelcome = FindViewById<RelativeLayout>(Resource.Id.layout_welcome);
            
            tvStatusText = FindViewById<IconTextView>(Resource.Id.status_text);
            rlStatusLayout = FindViewById<RelativeLayout>(Resource.Id.status_layout);
            rlStatusLayout.Visibility = Android.Views.ViewStates.Gone;
            torSearchView = FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.menu_search);
            statusAnimShow = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_slide_in_top);
            listViewAnimShow = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_slide_in_bottom);
            statusAnimHide = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_fade_out);
            welcomeAnimShow = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_grow_fade_in_from_bottom);
            welcomeAnimShow.Duration = 3000;
            layoutWelcome.Animation = welcomeAnimShow;
            listViewAnimHide = AnimationUtils.LoadAnimation(this, Resource.Animation.abc_slide_out_bottom);
            Iconify.with(new MaterialModule());
            layoutWelcome.Visibility = ViewStates.Visible;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            //Check network connection
            //var current = Connectivity.NetworkAccess;
            //if (current != NetworkAccess.Internet)
            //{
            // Toast.MakeText(this, "No internet connection detected", ToastLength.Short).Show();
            // }
        }

        protected override void OnStart()
        {
           // Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            //layoutWelcome.Visibility = ViewStates.Visible;
            base.OnStart();
        }

        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;
            if (access == NetworkAccess.None)
                Toast.MakeText(this, "No internet connection detected", ToastLength.Short).Show();

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
            Android.Widget.Toast.MakeText(this, t.Name, Android.Widget.ToastLength.Short).Show();
            torrentDialog = new Dialog(this);
            torrentDialog.SetContentView(Resource.Layout.TorrentDetailDialog);
            torrentDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
            torSearchView.ClearFocus();
            torSearchView.SetQuery("", false);
            //MenuItemCompat.CollapseActionView(item);
            torrentDialog.Show();
            torrentDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.WrapContent);
            torrentDialog.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);;
            tvTorName = torrentDialog.FindViewById<TextView>(Resource.Id.tvTorrentName);
            tvTorUploaded = torrentDialog.FindViewById<TextView>(Resource.Id.tvTorrentUploaded);
            tvTorName.Text = t.Name;
            tvTorUploaded.Text = t.Uploaded;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            
            MenuInflater.Inflate(Resource.Menu.toolmenu, menu);
            var item = menu.FindItem(Resource.Id.menu_search);
            var searchItem = MenuItemCompat.GetActionView(item);
            torSearchView = searchItem.JavaCast<Android.Support.V7.Widget.SearchView>();
            torSearchView.SetIconifiedByDefault(false);
            torSearchView.QueryHint = "Torrent Search...";
            //torSearchView.SubmitButtonEnabled = true;
            var simgid = torSearchView.Context.Resources.GetIdentifier("search_mag_icon", "id", PackageName);
            var id = torSearchView.Context.Resources.GetIdentifier("search_src_text", "id", PackageName);
            var searchEditText = torSearchView.FindViewById<EditText>(id);
            var searchIcon = torSearchView.FindViewById<ImageView>(simgid);
            
            searchIcon.SetColorFilter(Android.Graphics.Color.White);
            searchEditText.SetTextColor(Android.Graphics.Color.White);
            searchEditText.SetHintTextColor(Android.Graphics.Color.White);

            torSearchView.QueryTextSubmit += (s, e) =>
            {
                string query = e.NewText.ToString();
                rlStatusLayout.Visibility = ViewStates.Gone;
                listView.Visibility = ViewStates.Gone;
                Thread thread = new Thread(() =>
                {
                    UserDialogs.Instance.ShowLoading("Searching, Please Wait …", MaskType.Black);
                    torrents = Tpb.Search(new TpbQuery(query, 0, TpbQueryOrder.BySeeds,TpbTorrentCategory.All));
                    if (torrents.Count == 0)
                    {
                        RunOnUiThread(() => Toast.MakeText(this, "No results found for " + query, ToastLength.Short).Show());
                        
                    }
                    else
                    {
                        
                        listViewAnimShow.Duration = 500;
                        rlStatusLayout.StartAnimation(statusAnimShow);
                        listView.StartAnimation(listViewAnimShow);
                        RunOnUiThread(() => rlStatusLayout.Visibility = ViewStates.Visible);
                        RunOnUiThread(() => listView.Visibility = ViewStates.Visible);
                        RunOnUiThread(() => listView.Adapter = new TorListAdapter(this, torrents));
                        
                    };
                    
                    UserDialogs.Instance.HideLoading();
                }); ;
                thread.Start();
                torSearchView.ClearFocus();
                torSearchView.SetQuery("", false);
                MenuItemCompat.CollapseActionView(item);
                tvStatusText.Text = "Search results for: " + e.NewText.ToString();
                //tvStatusText.Text = "{md_accessibility}";
                layoutWelcome.Visibility = ViewStates.Gone;
                
                e.Handled = true;
            };
            //return true;
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToString() == "Sea")
            {
                
                searchDialog = new Dialog(this);
                
                searchDialog.SetContentView(Resource.Layout.searchpopup);
                searchDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
                //searchDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
                
                searchDialog.Show();
                searchDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.WrapContent);
                
                searchDialog.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
               
                spinnerCategory = searchDialog.FindViewById<Spinner>(Resource.Id.spinner_category);
                //spinnerProvider = searchDialog.FindViewById<Spinner>(Resource.Id.spinner_provider);
                //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

                var adapterCategory = ArrayAdapter.CreateFromResource(this, Resource.Array.categories, Android.Resource.Layout.SimpleSpinnerItem);
                adapterCategory.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinnerCategory.Adapter = adapterCategory;

                //var adapterProvider = ArrayAdapter.CreateFromResource(this, Resource.Array.providers, Android.Resource.Layout.SimpleSpinnerItem);
                //adapterProvider.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                //spinnerProvider.Adapter = adapterProvider;

                btnSearch = searchDialog.FindViewById<Button>(Resource.Id.btnsearch);
                btnSearch.Click += BtnSearch_Click;
                
            }

            if (item.TitleFormatted.ToString() == "Searc")
            {
                SetContentView(Resource.Layout.searchpopup);


            }
                //Toast.MakeText(this, "Action selected: " + item.TitleFormatted, ToastLength.Short).Show();
                return base.OnOptionsItemSelected(item);
        }
        private void BtnSearch_Click(object sender, System.EventArgs e)
        {
            
            edtSearchQuery = searchDialog.FindViewById<EditText>(Resource.Id.edtsearch);
            string query = edtSearchQuery.Text.ToString();
            Thread thread = new Thread(() =>
            {
                UserDialogs.Instance.ShowLoading("Please Wait …", MaskType.Black);
                torrents = Tpb.Search(new TpbQuery(query));
                RunOnUiThread(() => listView.Adapter = new TorListAdapter(this, torrents));
                UserDialogs.Instance.HideLoading();
            }); ;
            thread.Start();
            searchDialog.Dismiss();
            searchDialog.Hide(); 
        }
        //public void BtnClearSearch_Click(object sender, System.EventArgs e)
        //{
        //    listView.SetAdapter(null);
        //    listView.StartAnimation(listViewAnimHide);
        //    rlStatusLayout.StartAnimation(statusAnimHide);
        //    listView.Visibility = ViewStates.Gone;
        //    rlStatusLayout.Visibility = Android.Views.ViewStates.Gone;
        //    var back = this.GetDrawable(Resource.Drawable.buttonrect);
        //    toolbar.Background = back;
        //    layoutWelcome.Visibility = ViewStates.Visible;
        //}
    }
}