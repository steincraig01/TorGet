using Android.App;
using Android.Graphics;
using Android.Views;
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
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
            view.FindViewById<TextView>(Resource.Id.tvtorname).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.tvtorsize).Text = item.Size; 
            view.FindViewById<TextView>(Resource.Id.tvtoruploaded).Text = item.Uploaded;
            if (item.Uled == null)
                view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = "Anonymous";
            if (item.Uled != null)
                view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = item.Uled;
            view.FindViewById<TextView>(Resource.Id.tvtorseeds).Text = "Seeds: " + item.Seeds.ToString();
            view.FindViewById<TextView>(Resource.Id.tvtorleech).Text = "Leech: " + item.Leechers.ToString();
            view.FindViewById<TextView>(Resource.Id.tvtorcategory).Text = item.CategoryParent + " " + "(" + item.Category + ")";
            if (item.IsTrusted == true)
                view.FindViewById<ImageView>(Resource.Id.imgstatus).SetImageResource(Resource.Drawable.trustblue16);
            if (item.IsVip == true)
                view.FindViewById<ImageView>(Resource.Id.imgstatus).SetImageResource(Resource.Drawable.trustgreen16);
            if (item.Comments == 1)
                view.FindViewById<ImageView>(Resource.Id.imgcomments).SetImageResource(Resource.Drawable.commentyellow16);
            view.Elevation = 10;
            
            return view;
        }
    }
}