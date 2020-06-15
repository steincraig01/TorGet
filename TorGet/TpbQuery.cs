namespace TorGet
{
    public class TpbQuery
    {
        public TpbQueryOrder Order { get; set; }
        public int Category { get; set; }
        public int Page { get; set; }
        public string Term { get; set; }

        public TpbQuery(string term, int page = 0)
            : this(term, page, TpbQueryOrder.ByDefault, TpbTorrentCategory.All) { }
        public TpbQuery(string term, int page, int category)
            : this(term, page, TpbQueryOrder.ByDefault, category) { }
        public TpbQuery(string term, int page, TpbQueryOrder order)
            : this(term, page, order, TpbTorrentCategory.All) { }
        public TpbQuery(string term, int page, TpbQueryOrder order, int category)
        {
            Category = category;
            Order = order;
            Page = page;
            Term = term;
        }

        public string TranslateToUrl()
        {
            return string.Format(Constants.UrlSearch, Constants.UrlTpb[0], Term, Page.ToString(), ((int)Order).ToString(), Category.ToString());
        }
    }
}