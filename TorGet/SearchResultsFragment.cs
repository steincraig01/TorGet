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
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;
using JoanZapata.XamarinIconify.Widget;
using Acr.UserDialogs;
using Android.Views.Animations;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TorGet
{
    public class SearchResultsFragment : Android.Support.V4.App.Fragment
    {

        ListView lvResults;
        Animation listViewAnimShow;
        IconTextView tvSearchQuery;
        RelativeLayout rlStatus;



        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.search_results_fragment, container, false);
            MainActivity.Instance.toolbar.SetTitle(Resource.String.search_results_title);
            MainActivity.Instance.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            MainActivity.Instance.SupportActionBar.SetDisplayShowHomeEnabled(true);
            MainActivity.Instance.toolbar.Visibility = Android.Views.ViewStates.Visible;
            rlStatus = view.FindViewById<RelativeLayout>(Resource.Id.rlSearchResultsHeader);
            lvResults = view.FindViewById<ListView>(Resource.Id.lvSearchResults);
            tvSearchQuery = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvSearchQuery);

            var filterButton = view.FindViewById<ImageButton>(Resource.Id.ibFilters);
            filterButton.SetImageDrawable(new IconDrawable(Application.Context, MaterialCommunityIcons.mdi_filter_variant.ToString()).actionBarSize().colorRes(Resource.Color.colorTextWhite));
            filterButton.Click += MainActivity.Instance.ShowFilterDialog;

            listViewAnimShow = AnimationUtils.LoadAnimation(Application.Context, Resource.Animation.abc_slide_in_bottom);
            lvResults.ItemClick += OnListItemClick;
            if (Arguments != null)
            {
                string searchQuery = Arguments.GetString("searchQuery");
                PerformSearch(searchQuery, MainActivity.Instance.searchCategory, MainActivity.Instance.searchOrder);
            }

            return view;
        }


        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = MainActivity.Instance.torrents[e.Position];
            //Android.Widget.Toast.MakeText(this, t.Name, Android.Widget.ToastLength.Short).Show();
            Bundle mybundle = new Bundle();
            mybundle.PutString("torname", t.Name);
            mybundle.PutString("magnet", t.Magnet);
            mybundle.PutString("pageurl", t.PageUrl);
            //mybundle.PutString("torname", t.Name);
            TorDetailDialogFragment modalBottomSheet = new TorDetailDialogFragment();
            modalBottomSheet.Arguments = mybundle;
            modalBottomSheet.Show(MainActivity.Instance.SupportFragmentManager, "modalMenu");


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

            //torSearchView.ClearFocus();
            //torSearchView.SetQuery("", false);

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

        

        public async void PerformSearch(string searchQuery, int searchCategory, TpbQueryOrder searchOrder)
        {
            if (MainActivity.Instance.IsConnected == true)
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var taskList = new List<Task>();
                    MainActivity.Instance.torrents.Clear();
                    UserDialogs.Instance.ShowLoading("Searching…");
                    Task tpbTask = new Task(() => MainActivity.Instance.torrents.AddRange(Tpb.Search(new TpbQuery(searchQuery, 0, searchOrder, MainActivity.Instance.searchCategory))));
                    Task ytsTask = new Task(() => MainActivity.Instance.torrents.AddRange(YifiYts.Search(searchQuery)));
                    taskList.Add(tpbTask);
                    tpbTask.Start();
                    if (MainActivity.Instance.searchCategory == 0 || MainActivity.Instance.searchCategory == 200)
                    {
                        taskList.Add(ytsTask);
                        ytsTask.Start();
                    }
                       
                    await Task.WhenAll(taskList.ToArray());
                    UserDialogs.Instance.HideLoading();
                    if (MainActivity.Instance.torrents.Count == 0)
                    {
                        MainActivity.Instance.RunOnUiThread(() => Toast.MakeText(Application.Context, "No results found for " + searchQuery, ToastLength.Short).Show());
                    }
                    else
                    {
                        MainActivity.Instance.torrents.Sort((x, y) => y.Seeds.CompareTo(x.Seeds));
                        lvResults.Adapter = new TorListAdapter(MainActivity.Instance, MainActivity.Instance.torrents);
                    };
                    stopwatch.Stop();
                    var ts = System.Math.Round(stopwatch.Elapsed.TotalSeconds,1);
                    rlStatus.Visibility = ViewStates.Visible;
                    tvSearchQuery.Text = "{md_search 24dp #007ACC}  " + Arguments.GetString("searchQuery");
                    //tvSearchQuery.Text = tvSearchQuery.Text + " " + ts.ToString();
                }
                catch(System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }  
            }
            else
            {
                Toast.MakeText(Application.Context, "No internet connection detected!", ToastLength.Long).Show();
            };

        }
    }
}