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
        Spinner spinnerProvider;
        TextView tvTorName;
        TextView tvTorUploaded;
        TextView tvTorUploader;
        TextView tvTorCategory;
        TextView tvTorSize;
        TextView tvTorSeedLeech;
        RelativeLayout rlStatusLayout;
        Android.Support.V7.Widget.Toolbar toolbar;
        TextView tvStatusText;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            listView = FindViewById<ListView>(Resource.Id.lvresults);
            listView.ItemClick += OnListItemClick;
            listView.SetPadding(20, 20, 20, 10);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "TorGet";
            var back = this.GetDrawable(Resource.Drawable.buttonrect);
            toolbar.Background = back;
            tvStatusText = FindViewById<TextView>(Resource.Id.status_text);
            rlStatusLayout = FindViewById<RelativeLayout>(Resource.Id.status_layout);
            rlStatusLayout.Visibility = Android.Views.ViewStates.Gone;
            torSearchView = FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.menu_search);
            //Check network connection
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {
                Toast.MakeText(this, "No internet connection detected", ToastLength.Short).Show();
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
            Android.Widget.Toast.MakeText(this, t.Name, Android.Widget.ToastLength.Short).Show();
            torrentDialog = new Dialog(this);
            torrentDialog.SetContentView(Resource.Layout.TorrentDetailDialog);
            torrentDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
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
            torSearchView.QueryHint = "Torrent Search...";
            torSearchView.SubmitButtonEnabled = true;
            var id = torSearchView.Context.Resources.GetIdentifier("search_src_text", "id", PackageName);
            var searchEditText = torSearchView.FindViewById<EditText>(id);
            searchEditText.SetTextColor(Android.Graphics.Color.White);
            searchEditText.SetHintTextColor(Android.Graphics.Color.WhiteSmoke);
            
            torSearchView.QueryTextSubmit += (s, e) =>
            {
                string query = e.NewText.ToString();
                Thread thread = new Thread(() =>
                {
                    UserDialogs.Instance.ShowLoading("Searching, Please Wait …", MaskType.Black);
                    torrents = Tpb.Search(new TpbQuery(query, 0, TpbQueryOrder.BySize,TpbTorrentCategory.All));
                   
                    RunOnUiThread(() => listView.Adapter = new TorListAdapter(this, torrents));
                    UserDialogs.Instance.HideLoading();
                }); ;
                thread.Start();
                torSearchView.ClearFocus();
                torSearchView.SetQuery("", false);
                MenuItemCompat.CollapseActionView(item);
                toolbar.SetBackgroundColor(Android.Graphics.Color.ParseColor("#007ACC"));
                tvStatusText.Text = "Search results for: " + e.NewText.ToString();
                rlStatusLayout.Visibility = Android.Views.ViewStates.Visible;
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
    }
}