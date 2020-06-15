using Acr.UserDialogs;
using Android.App;
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
        Button btnCancelSearch;
        EditText edtSearchQuery;
        Dialog searchDialog;
        Spinner spinner;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            listView = FindViewById<ListView>(Resource.Id.lvresults);
            listView.ItemClick += OnListItemClick;
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
                searchDialog = new Dialog(this);
                searchDialog.SetContentView(Resource.Layout.searchpopup);
                searchDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
                //searchDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
                
                searchDialog.Show();
                searchDialog.Window.SetLayout(LayoutParams.FillParent, LayoutParams.WrapContent);
                
                searchDialog.Window.SetBackgroundDrawableResource(Resource.Color.transparent);
                //btnCancelSearch = searchDialog.FindViewById<Button>(Resource.Id.btncancelsearch);
                spinner = searchDialog.FindViewById<Spinner>(Resource.Id.spinner);
                //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
                
                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.categories, Android.Resource.Layout.SimpleSpinnerItem);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinner.Adapter = adapter;
                btnSearch = searchDialog.FindViewById<Button>(Resource.Id.btnsearch);
                //btnCancelSearch.Click += BtnCancelSearch_Click;
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

        private void BtnCancelSearch_Click(object sender, System.EventArgs e)
        {
            searchDialog.Dismiss();
            searchDialog.Hide();
        }
    }

}