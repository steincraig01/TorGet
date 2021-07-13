using Android.App;

using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System.Collections.Generic;
using JoanZapata.XamarinIconify;
using JoanZapata.XamarinIconify.Fonts;


namespace TorGet
{
    public class TorListAdapter : BaseAdapter<Torrent>
    {
        List<Torrent> items;
        Activity context;




        public TorListAdapter(Activity context, List<Torrent> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Torrent this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }




        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            //view = null;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
                view.Tag = new ViewHolder()
                {
                    Name = view.FindViewById<TextView>(Resource.Id.tvtorname),
                    Size = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorsize),
                    Uploaded = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtoruploaded),
                    Provider = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorprovider),
                    Seeds = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorseeds),
                    Leechers = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorleech),
                    Category = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory),
                    Trusted = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtortrusted),
                    HeaderIcon = view.FindViewById<ImageView>(Resource.Id.ivHeaderIcon)
                };
            }
            var holder = (ViewHolder)view.Tag;
            holder.Name.Text = item.Name;
            holder.Size.Text = "{mdi_harddisk 18dp #007ACC} " + item.Size;
            holder.Uploaded.Text = "{mdi_calendar_clock 18dp #007ACC} " + item.Uploaded;
            holder.Seeds.Text = "{md_file_upload 18dp #007ACC} " + item.Seeds.ToString();
            holder.Leechers.Text = "{md_file_download 18dp #007ACC} " + item.Leechers.ToString();
            holder.Provider.Text = "{mdi_web 18dp #007ACC} " + item.Provider;
            //view.FindViewById<TextView>(Resource.Id.tvtorname).Text = item.Name;
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorsize).Text = item.Size;
            //view.FindViewById<TextView>(Resource.Id.tvtoruploaded).Text = item.Uploaded;
            //view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = "Anonymous";

            //view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = item.Uled;
            //view.FindViewById<TextView>(Resource.Id.tvtorseeds).Text = item.Seeds.ToString();
            //view.FindViewById<TextView>(Resource.Id.tvtorleech).Text = item.Leechers.ToString();
            if (item.CategoryParent == "Audio")
            {
                holder.Category.Text = "{md_dashboard 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
                holder.HeaderIcon.SetImageDrawable(new IconDrawable(Application.Context, MaterialCommunityIcons.mdi_music_box_outline.ToString()).sizeDp(48).colorRes(Resource.Color.colorAppBlue));
            }    
                //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_music_note 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Video")
            {
                holder.Category.Text = "{md_dashboard 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
                holder.HeaderIcon.SetImageDrawable(new IconDrawable(Application.Context, MaterialIcons.md_ondemand_video.ToString()).sizeDp(48).colorRes(Resource.Color.colorAppBlue));
            }
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_movie 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Applications")
            {
                holder.Category.Text = "{md_dashboard 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
                holder.HeaderIcon.SetImageDrawable(new IconDrawable(Application.Context, MaterialIcons.md_desktop_windows.ToString()).sizeDp(48).colorRes(Resource.Color.colorAppBlue));
            }
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_apps 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Games")
            {
                holder.Category.Text = "{md_dashboard 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
                holder.HeaderIcon.SetImageDrawable(new IconDrawable(Application.Context, MaterialCommunityIcons.mdi_google_controller.ToString()).sizeDp(48).colorRes(Resource.Color.colorAppBlue));
            }    
                //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_games 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Porn")
                holder.Category.Text = "{md_dashboard 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_local_play 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Other")
            {
                holder.Category.Text = "{md_dashboard 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
                holder.HeaderIcon.SetImageDrawable(new IconDrawable(Application.Context, MaterialIcons.md_picture_as_pdf.ToString()).sizeDp(48).colorRes(Resource.Color.colorAppBlue));
            }    
                //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_more 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            // view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.IsTrusted == true || item.IsVip == true)
            {
                holder.Trusted.Text = "{md_verified_user 10dp #66C10A} Trusted";
            }
            else
            {
                holder.Trusted.Visibility = ViewStates.Gone;
            };
                //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtortrusted).Text = "{md_verified_user 18dp #007ACC} Trusted";



            view.Elevation = 4;

            //Animation myAnimation = AnimationUtils.LoadAnimation(Application.Context, Resource.Animation.abc_grow_fade_in_from_bottom);
            //myAnimation.Duration = 100;
            //view.StartAnimation(myAnimation);

            return view;
        }
    }
}