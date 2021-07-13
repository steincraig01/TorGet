using Android.OS;
using Android.Widget;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Android.App;

namespace TorGet
{
    public class Tpb
    {
        public static List<Torrent> torrentList = new List<Torrent>();
        public static List<Torrent> Search(TpbQuery query)
        {
            //clear torrentlist each search?
            torrentList.Clear();
            
            //List<Torrent> result = new List<Torrent>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            try
            {
                doc = web.Load(query.TranslateToUrl());
            }
            catch (Exception ex)
            {
                
                // Debug.WriteLine(ex.ToString());
            }
            if (doc.Text != null)
            {

                HtmlNode table = doc.DocumentNode.SelectSingleNode("//table");
                // HtmlNode nNode in table.Descendants("tr").Where(f => f.GetAttributeValue("class", "") != "header" && f.GetAttributeValue("class", "") != "alt"))
                foreach (HtmlNode trNode in table.Descendants("tr").Where(f => f.GetAttributeValue("class", "") != "header"))
                {
                    Torrent torrent = new Torrent();
                    foreach (HtmlNode tdNode in trNode.Descendants("td"))
                    {
                        if (tdNode.GetAttributeValue("class", "").Contains("vertTh"))
                        {
                            torrent.CategoryParent = tdNode.Descendants("a").ElementAt(0).InnerText;
                            torrent.Category = tdNode.Descendants("a").ElementAt(1).InnerText;
                        }
                    }
                    foreach (HtmlNode aNode in trNode.Descendants("a"))
                    {
                        // First a: Everything except seed/leechers and categories
                        if (aNode.GetAttributeValue("class", "").Contains("detLink"))
                        {
                            torrent.PageUrl = aNode.Attributes.ElementAt(0).Value;
                            torrent.Name = aNode.InnerText;
                        }
                        if (aNode.GetAttributeValue("href", "").Contains("magnet:?"))
                        {
                            torrent.Magnet = aNode.Attributes.ElementAt(0).Value;
                        }
                        if (aNode.GetAttributeValue("class", "").Contains("detDesc"))
                        {
                            torrent.Uled = aNode.InnerText;
                        }
                    }
                    foreach (HtmlNode imgNode in trNode.Descendants("img"))
                    {
                        if (imgNode.GetAttributeValue("alt", "").Contains("Trusted"))
                            torrent.IsTrusted = true;
                        if (imgNode.GetAttributeValue("alt", "").Contains("VIP"))
                            torrent.IsVip = true;
                        if (imgNode.GetAttributeValue("alt", "").Contains("comments"))
                            //todo: get comment count
                            torrent.Comments = 1;
                    }
                    foreach (HtmlNode fontNode in trNode.Descendants("font"))
                    {
                        string[] parameters = fontNode.InnerText.Replace("&nbsp;", " ").Split(',');
                        foreach (var parameter in parameters)
                        {
                            if (parameter.Trim().StartsWith("Uploaded"))
                            {
                                torrent.Uploaded = parameter.Trim().Remove(0, 9);
                                if (torrent.Uploaded.StartsWith("Today"))
                                    torrent.Uploaded = DateTime.Now.ToString("d/M/yyyy");
                                if (torrent.Uploaded.StartsWith("Y-day"))
                                    torrent.Uploaded = DateTime.Now.AddDays(-1).ToString("d/M/yyyy");
                                if (torrent.Uploaded.Contains("ago"))
                                    torrent.Uploaded = DateTime.Now.ToString("d/M/yyyy");
                                //last 3 days
                                if (torrent.Uploaded.IndexOf("-") == 2 && torrent.Uploaded.IndexOf(":") == 8)
                                {
                                    var m = torrent.Uploaded.Substring(0, 2);
                                    var d = torrent.Uploaded.Substring(3, 2);
                                    var y = DateTime.Now.Year;
                                    torrent.Uploaded = d + "/" + m + "/" + y;
                                }
                                //format normal date
                                if (torrent.Uploaded.IndexOf("-") == 2 && torrent.Uploaded.Length == 10)
                                {
                                    var m = torrent.Uploaded.Substring(0, 2);
                                    var d = torrent.Uploaded.Substring(3, 2);
                                    var y = torrent.Uploaded.Substring(6, 4);
                                    //torrent.Uploaded = "test";
                                    torrent.Uploaded = d + "/" + m + "/" + y;
                                }


                            }
                            //torrent.Uploaded = parameter.Trim().Replace("Uploaded", "");
                            if (parameter.Trim().StartsWith("Size"))
                            {
                                string[] size = parameter.Trim().Split(' ');
                                torrent.Size = string.Join(" ", size.Skip(1));
                                torrent.Size = torrent.Size.Replace("i", "");
                                
                            }
                        }
                    }

                    //Get seeds & Leech
                    var tick = 0;
                    HtmlNodeCollection childNodes = trNode.ChildNodes;
                    foreach (var n in childNodes)
                    {
                        if (n.NodeType == HtmlNodeType.Element)
                        {
                            if (n.GetAttributeValue("align", "") == "right")
                            {
                                tick++;
                                if (tick == 1)
                                    torrent.Seeds = Int32.Parse(n.InnerText);
                                if (tick == 2)
                                    torrent.Leechers = Int32.Parse(n.InnerText);
                            }
                        }
                    }

                    torrent.Provider = "TPB";

                    if (torrent.Name != null)
                        torrentList.Add(torrent);
                }
            }
            return torrentList;
        }
    }
}
