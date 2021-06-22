using Android.OS;
using Android.Widget;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Android.App;
using YTS;
using YTS.Models;

namespace TorGet
{
    public class YifiYts
    {
        public static List<Torrent> ytsTorrentList = new List<Torrent>();
        public static List<Torrent> Search(string query)
        {
            //clear torrentlist each search?
            ytsTorrentList.Clear();
            try
            {
                var API = new YTS.Services(new Uri("https://yts.mx/api/v2"));
                //System.Diagnostics.Debug.WriteLine(API.);
                var Response = API.GetMovieList(Limit: 50, Page: 1, MovieQuality.All, Query: query, SortBy: SortBy.Seeds, OrderBy: OrderBy.Decending);
                var Movies = Response.Data.Movies;
                //System.Diagnostics.Debug.WriteLine(Movies.Count().ToString());
                var trackers = new string[] // Creating Trackers For Torrent
                {
                    "udp://glotorrents.pw:6969/announce",
                    "udp://tracker.opentrackr.org:1337/announce",
                    "udp://torrent.gresille.org:80/announce",
                    "udp://tracker.openbittorrent.com:80",
                    "udp://tracker.coppersurfer.tk:6969",
                    "udp://tracker.leechers-paradise.org:6969",
                    "udp://p4p.arenabg.ch:1337",
                    "udp://tracker.internetwarriors.net:1337"
                };


                foreach (var movie in Movies)
                {
                    foreach (var t in movie.Torrents)
                    {
                        Torrent torrent = new Torrent();
                        
                        torrent.Name = movie.Title + " (" + movie.Year.ToString() + ") " + t.Quality + " " + t.Type.ToUpper() + " - YIFI";
                        torrent.Uploaded = t.DateUploaded.ToShortDateString();
                        torrent.Size = t.Size;
                        torrent.Uled = "YIFI";
                        torrent.Magnet = t.CreateMagnet(torrent.Name, trackers);
                        torrent.PageUrl = movie.URL;
                        torrent.Seeds = t.Seeds;
                        torrent.Leechers = t.Peers;
                        torrent.IsTrusted = true;
                        torrent.CategoryParent = "Video";
                        torrent.Category = "HD - Movies";
                        torrent.Provider = "YTS";
                        torrent.MovieID = movie.ID;
                        ytsTorrentList.Add(torrent);

                    }
                       //t.Quality == "1080p") // Checks If The Torrent Is 1080p
                            
                    

                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return ytsTorrentList;
        }
    }
}
