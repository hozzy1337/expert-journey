
using NewsWebSite.Models;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Controllers
{
    public class HomeController : Controller
    {
       //создает тестовые записи в таблице, онли для дебага
        [HttpGet]
        public ActionResult CreateLines(int n = 0)
        {
            var session = NHibernateHelper.OpenSession();
            for (int i = 1; i <= n; i++)
            {
                var a = new Article();
                a.Title = i.ToString();
                a.ShortDescription = i.ToString();
                session.Save(a);
            }
            return Content("ok");
        }
        //---------

        private ModelForListItemPage GetItems4ListItemPage(int Page, int NumberOfItemsOnPage)
        {
            if (Page < 1) Page = 1;
            var session = NHibernateHelper.OpenSession();
            var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
            int NumberOfPages = count / NumberOfItemsOnPage + 1;
            if (Page > NumberOfPages) Page = NumberOfPages;
            var FirstResultNum = (Page - 1) * NumberOfItemsOnPage;
            var criteria = session.CreateCriteria<Article>().SetFirstResult(FirstResultNum).SetMaxResults(NumberOfItemsOnPage).AddOrder(Order.Desc("Id"));
            var list = criteria.List<Article>();

            int NumOfPages = count / NumberOfItemsOnPage;
            ModelForListItemPage model = new ModelForListItemPage(NumberOfItemsOnPage, Page, list, NumOfPages + (count % NumberOfItemsOnPage == 0 ? 0 : 1));
            return model;
        }


        [HttpGet]
        public ActionResult Index(int Page = 1)
        {
            int NumberOfItemsOnPage;
            if (HttpContext.Request.Cookies["NumberOfItemsOnPage"].Value == null)
            {
                NumberOfItemsOnPage = 10;
                HttpContext.Response.Cookies["NumberOfItemsOnPage"].Value = "10";
            }
            else
            {
                try
                {
                    NumberOfItemsOnPage = Convert.ToInt32(HttpContext.Request.Cookies["NumberOfItemsOnPage"].Value);
                }
                catch
                {
                    NumberOfItemsOnPage = 10;
                }
            }
            var model = GetItems4ListItemPage(Page, NumberOfItemsOnPage);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string ItemsOnPageTB = "10", string BtnForItemsOnPBtn = "", string CurPage = "1", string CurItemOnPageCnt = "10")
        {
            int NewItemsOnPageNum = 10;
            int NewPageNum = 1;
            if (BtnForItemsOnPBtn == "OK")
            {
                try
                {
                    NewItemsOnPageNum = int.Parse(ItemsOnPageTB);
                    HttpContext.Response.Cookies["NumberOfItemsOnPage"].Value = NewItemsOnPageNum.ToString();
                }
                catch { }
                try
                {
                    int CurPageInt = int.Parse(CurPage);
                    int CurItemOnPageCntInt = int.Parse(CurItemOnPageCnt);
                    NewPageNum = (int)((double)(CurItemOnPageCntInt * (CurPageInt - 1) + (CurItemOnPageCntInt / 2)) / NewItemsOnPageNum) + 1;
                }
                catch
                {
                    NewPageNum = 1;
                }
            }
            return View("Index", GetItems4ListItemPage(NewPageNum, NewItemsOnPageNum));
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}