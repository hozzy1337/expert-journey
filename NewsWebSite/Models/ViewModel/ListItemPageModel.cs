using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.ViewModel
{
    public class ListItemPageModel
    {
        public int NumberOfItemsOnPage { get; set; }
        public PagedList<DemoArticle> ArticleList { get; set; }

        public ListItemPageModel(int NumberOfItemsOnPage, PagedList<DemoArticle> ArticleList)
        {
            this.NumberOfItemsOnPage = NumberOfItemsOnPage;
            this.ArticleList = ArticleList;
        }
    }
}