﻿using Android.App;

using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;


namespace TorGet
{
    public class ProviderListAdapter : BaseAdapter
    {
        System.String[] providers;
        Activity context;

        public ProviderListAdapter(Activity context, System.String[] providers)
            : base()
        {
            this.context = context;
            this.providers = providers;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return providers.Length; }
        }




        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var provider = providers[position];
            View view = convertView;

            view = context.LayoutInflater.Inflate(Resource.Layout.spinner_item_provider, null);
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.categoryName).Text = category.ToString();

            if (provider.ToString() == "All")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{md_dashboard 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "All";
            }
            if (provider.ToString() == "ThePirateBay")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{mdi_web 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "ThePirateBay";
            }

            if (provider.ToString() == "YIFY")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{mdi_web 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "YTS";
            }

            if (provider.ToString() == "KickassTorrents")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{mdi_web 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "KickassTorrents";
            }
            if (provider.ToString() == "RARBG")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{mdi_web 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "RARBG";
            }

            if (provider.ToString() == "1337x")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{mdi_web 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "1337x";
            }
            if (provider.ToString() == "EZTV")
            {
                view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.providerIcon).Text = "{mdi_web 18dp #007ACC}";
                view.FindViewById<TextView>(Resource.Id.providerName).Text = "EZTV";
            }




            //view = null;
            //if (view == null)
            //{
            //    view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
            //view.Tag = new ViewHolder()
            //{
            //    Name = view.FindViewById<TextView>(Resource.Id.tvtorname),
            //    Size = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorsize),
            //    Uploaded = view.FindViewById<TextView>(Resource.Id.tvtoruploaded),
            //    Uploader = view.FindViewById<TextView>(Resource.Id.tvtoruled),
            //    Seeds = view.FindViewById<TextView>(Resource.Id.tvtorseeds),
            //    Leechers = view.FindViewById<TextView>(Resource.Id.tvtorleech),
            //    Category = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory),
            //    Trusted = view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtortrusted)
            //};
            //}
            //var holder = (ViewHolder)view.Tag;
            //holder.Name.Text = item.Name;
            //holder.Size.Text = "{mdi_content_save 18dp #007ACC} " + item.Size;
            //holder.Uploaded.Text = item.Uploaded;
            //holder.Seeds.Text = item.Seeds.ToString();
            //holder.Leechers.Text = item.Leechers.ToString();

            //view.FindViewById<TextView>(Resource.Id.tvtorname).Text = item.Name;
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorsize).Text = item.Size;
            //view.FindViewById<TextView>(Resource.Id.tvtoruploaded).Text = item.Uploaded;
            //if (item.Uled == null)
            //holder.Uploader.Text = "Anonymous";
            //view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = "Anonymous";
            //if (item.Uled != null)
            //holder.Uploader.Text = item.Uled;
            //view.FindViewById<TextView>(Resource.Id.tvtoruled).Text = item.Uled;
            //view.FindViewById<TextView>(Resource.Id.tvtorseeds).Text = item.Seeds.ToString();
            //view.FindViewById<TextView>(Resource.Id.tvtorleech).Text = item.Leechers.ToString();
            //if (item.CategoryParent == "Audio")
            //holder.Category.Text = "{md_music_note 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_music_note 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //if (item.CategoryParent == "Video")
            //    holder.Category.Text = "{md_movie 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_movie 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //if (item.CategoryParent == "Applications")
            //    holder.Category.Text = "{md_apps 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_apps 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //if (item.CategoryParent == "Games")
            //    holder.Category.Text = "{md_games 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_games 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //if (item.CategoryParent == "Porn")
            //    holder.Category.Text = "{md_local_play 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_local_play 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //if (item.CategoryParent == "Other")
            //   holder.Category.Text = "{md_more 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = "{md_more 18dp #007ACC} " + item.CategoryParent + " " + "(" + item.Category + ")";
            // view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtorcategory).Text = item.CategoryParent + " " + "(" + item.Category + ")";
            //if (item.IsTrusted == true || item.IsVip == true)
            //{
            //    holder.Trusted.Text = "{md_verified_user 18dp #66C10A} Trusted";
            //}
            //else
            //{
            //    holder.Trusted.Text = "{md_help 18dp} Unknown";
            //};
            //view.FindViewById<JoanZapata.XamarinIconify.Widget.IconTextView>(Resource.Id.tvtortrusted).Text = "{md_verified_user 18dp #007ACC} Trusted";



            //view.Elevation = 3;

            //Animation myAnimation = AnimationUtils.LoadAnimation(Application.Context, Resource.Animation.abc_grow_fade_in_from_bottom);
            //myAnimation.Duration = 100;
            //view.StartAnimation(myAnimation);

            return view;
        }


    }
}