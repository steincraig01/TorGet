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

namespace TorGet
{
    public class SearchResultsFragment : Android.Support.V4.App.Fragment
    {

        ListView lvResults;
        Animation listViewAnimShow;



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
            lvResults = view.FindViewById<ListView>(Resource.Id.lvSearchResults);
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
            mybundle.PutString("pageurl", t.TpbPage);
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

        public void PerformSearch(string searchQuery, int searchCategory, TpbQueryOrder searchOrder)
        {
            if (MainActivity.Instance.IsConnected == true)
            {

                System.Threading.Thread thread = new System.Threading.Thread(() =>
                {

                    UserDialogs.Instance.ShowLoading("Searching, Please Wait …", MaskType.Black);
                    MainActivity.Instance.torrents = Tpb.Search(new TpbQuery(searchQuery, 0, searchOrder, searchCategory));
                    if (MainActivity.Instance.torrents.Count == 0)
                    {
                        MainActivity.Instance.RunOnUiThread(() => Toast.MakeText(Application.Context, "No results found for " + searchQuery, ToastLength.Short).Show());
                    }
                    else
                    {
                        //listViewAnimShow.Duration = 200;

                        lvResults.StartAnimation(listViewAnimShow);

                        //MainActivity.Instance.RunOnUiThread(() => listView.Visibility = ViewStates.Visible);
                        MainActivity.Instance.RunOnUiThread(() => lvResults.Adapter = new TorListAdapter(MainActivity.Instance, MainActivity.Instance.torrents));
                    };

                    UserDialogs.Instance.HideLoading();
                }); ;
                thread.Start();
                //torSearchView.ClearFocus();

                //torSearchView.SetQuery("", false);
                //MenuItemCompat.CollapseActionView(item);
                //tvStatusText.Text = "Search results for: " + e.NewText.ToString();
                //tvStatusText.Text = MaterialIcons.md_dis;
                //layoutWelcome.Visibility = ViewStates.Gone;

                //e.Handled = true;
            }
            else
            {
                //torSearchView.ClearFocus();
                Toast.MakeText(Application.Context, "No internet connection detected!", ToastLength.Long).Show();
            };

        }
    }
}