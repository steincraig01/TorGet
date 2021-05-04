using Android.App;
using Android.Graphics;

using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System.Collections.Generic;



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
            view = null;
            view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
            view.FindViewById<TextView>(Resource.Id.tvtorname).Text = item.Name;
            view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorsize).Text = "{md_save 18dp #007ACC} " + item.Size; 
            view.FindViewById<TextView>(Resource.Id.tvtoruploaded).Text = item.Uploaded;
            if (item.Uled == null)
                view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = "Anonymous";
            if (item.Uled != null)
                view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = item.Uled;
            view.FindViewById<TextView>(Resource.Id.tvtorseeds).Text = item.Seeds.ToString();
            view.FindViewById<TextView>(Resource.Id.tvtorleech).Text = item.Leechers.ToString();
            if (item.CategoryParent == "Audio")
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_music_note 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Video")
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_movie 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Applications")
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_apps 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Games")
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_games 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Porn")
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_local_play 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.CategoryParent == "Other")
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_more 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            // view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.IsTrusted == true || item.IsVip == true)
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtortrusted).Text = "{md_verified_user 18dp #007ACC} Trusted";


            view.Elevation = 3;
            
            Animation myAnimation = AnimationUtils.LoadAnimation(Application.Context, Resource.Animation.abc_grow_fade_in_from_bottom);
            //myAnimation.Duration = 100;
            view.StartAnimation(myAnimation);

            return view;
        }
    }
}