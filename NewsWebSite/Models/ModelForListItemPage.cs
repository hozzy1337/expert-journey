using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class ModelForListItemPage
    {


        public int NumberOfItemsOnPage { get; set; }
        public int PageNumber { get; set; }
        public IList<DemoArticle> ArticleList {get; set;}
        public int NumberOfPages { get; set; }
        public ModelForListItemPage(int NumberOfItemsOnPage, int PageNumber, IList<DemoArticle> ArticleList, int NumberOfPages)
        {
            this.NumberOfItemsOnPage = NumberOfItemsOnPage;
            this.PageNumber = PageNumber;
            this.ArticleList = ArticleList;
            this.NumberOfPages = NumberOfPages;
        }

     
    }
}