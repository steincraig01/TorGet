using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using static Android.App.ActionBar;


namespace TorGet
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        List<Torrent> torrents;
        ListView listView;
        Button btnSearch;
        EditText edtSearchQuery;
        Dialog searchDialog;
        Dialog torrentDialog;
        Spinner spinnerCategory;
        Spinner spinnerProvider;
        LinearLayout mainContentLayout;
        LinearLayout emptyView;
        TextView tvTorName;
        TextView tvTorUploaded;
        TextView tvTorUploader;
        TextView tvTorCategory;
        TextView tvTorSize;
        TextView tvTorSeedLeech;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            listView = FindViewById<ListView>(Resource.Id.lvresults);
            //mainContentLayout = FindViewById<LinearLayout>(Resource.Id.content_layout);
            //emptyView = FindViewById<LinearLayout>(Resource.Id.ListViewMessage);
            //mainContentLayout.AddView(FindViewById<>(Resource.Layout.EmptyView));
            //listView.EmptyView = FindViewById<TextView>(Resource.Id.tvLVM) ;
            //listView.AddView(FindViewById<TextView>(Resource.Id.tvLVM));
            
            listView.ItemClick += OnListItemClick;
            listView.SetPadding(20, 20, 20, 10);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "TorGet";

            //ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout, objects);
            //View headerView = this.LayoutInflater.Inflate(Resource.Layout.listview_header, null);
            //listView.AddHeaderView(headerView);

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
            //searchDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
            
            torrentDialog.Show();
            torrentDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.WrapContent);
            //torrentDialog.Window.SetBackgroundDrawableResource(Resource.Color.transparent);
            tvTorName = torrentDialog.FindViewById<TextView>(Resource.Id.tvTorrentName);
            tvTorName.Text = t.Name;
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            
            MenuInflater.Inflate(Resource.Menu.toolmenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToString() == "Search")
            {
                //listView.Visibility = Android.Views.ViewStates.Gone;
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