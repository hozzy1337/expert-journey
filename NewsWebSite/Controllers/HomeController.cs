
using NewsWebSite.Models;
using Newtonsoft.Json;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Controllers
{
    public class HomeController : Controller
    {
        const int NumberOfItemsOnPage = 10;
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
                a.CreateDate = DateTime.Now;
                a.LastUpdateDate = DateTime.Now;
                session.BeginTransaction();
                session.Save(a);
                session.Transaction.Commit();
            }
            return Content("ok");
        }
        ////////////////////////////////////////
        [HttpPost]
        public bool AddNewArticle(Article add , HttpPostedFileBase loadfile)
        {
            try
            {
                var session = NHibernateHelper.OpenSession();
                add.LastUpdateDate = DateTime.Now;
                add.CreateDate = DateTime.Now;
                if(loadfile!=null)
                {
                    string file_path = Path.GetFileName(loadfile.FileName);
                    add.Image = file_path;
                    loadfile.SaveAs(Server.MapPath("~/IMG/") + file_path);
                }
                session.BeginTransaction();
                session.Save(add);
                session.Transaction.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
/////////////////////////////////
        
        //---------

        //private ModelForListItemPage GetItems4ListItemPage(int Page)
        //{
        //    if (Page < 1) Page = 1;
        //    var session = NHibernateHelper.OpenSession();
        //    var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
        //    int NumberOfPages = count / NumberOfItemsOnPage + 1;
        //    if (Page > NumberOfPages) Page = NumberOfPages;
        //    var FirstResultNum = (Page - 1) * NumberOfItemsOnPage;
        //    var criteria = session.CreateCriteria<Article>().SetFirstResult(FirstResultNum).SetMaxResults(NumberOfItemsOnPage).AddOrder(Order.Desc("Id"));
        //    var list = criteria.List<Article>();

        //    int NumOfPages = count / NumberOfItemsOnPage;
        //    ModelForListItemPage model = new ModelForListItemPage(NumberOfItemsOnPage, Page, list, NumOfPages + (count % NumberOfItemsOnPage == 0 ? 0 : 1));
        //    return model;
        //}


        [HttpGet]
        public ActionResult Index(int Page = 1)
        {
            if (Page < 1) Page = 1;
            //var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
            var session = NHibernateHelper.OpenSession();
            var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value; //достаем количество записей в таблице
            var NumberOfPages = count / NumberOfItemsOnPage + (count % NumberOfItemsOnPage == 0 ? 0 : 1); //вычесляем количество страниц
            if (Page > NumberOfPages) Page = NumberOfPages;
            var list = session.CreateCriteria<Article>().SetFirstResult(0).SetMaxResults(NumberOfItemsOnPage * Page).AddOrder(Order.Desc("Id")).List<Article>();//достаем записи
           
            var model = new ModelForListItemPage(NumberOfItemsOnPage, Page, list, NumberOfPages); 
            return View(model);
        }

        [HttpPost]
        public string GetArticles(int Page)
        {
            if (Page < 1) Page = 1;
            var session = NHibernateHelper.OpenSession();
            var list = session.CreateCriteria<Article>().SetFirstResult(Page * NumberOfItemsOnPage).SetMaxResults(NumberOfItemsOnPage).AddOrder(Order.Desc("Id")).List<Article>();
            var json = JsonConvert.SerializeObject(list);
            return json;
        }
        
        //[HttpPost]
        //public ActionResult Index(string ItemsOnPageTB = "10", string BtnForItemsOnPBtn = "", string CurPage = "1", string CurItemOnPageCnt = "10")
        //{
        //    int NewItemsOnPageNum = 10;
        //    int NewPageNum = 1;
        //    if (BtnForItemsOnPBtn == "OK")
        //    {
        //        try
        //        {
        //            NewItemsOnPageNum = int.Parse(ItemsOnPageTB);
        //            if (NewItemsOnPageNum <= 0 || NewItemsOnPageNum > 100) NewItemsOnPageNum = 10;
        //            HttpContext.Response.Cookies["NumberOfItemsOnPage"].Value = NewItemsOnPageNum.ToString();
        //        }
        //        catch {; }
        //        try
        //        {
        //            int CurPageInt = int.Parse(CurPage);
        //            int CurItemOnPageCntInt = int.Parse(CurItemOnPageCnt);
        //            NewPageNum = (int)((double)(CurItemOnPageCntInt * (CurPageInt - 1) + (CurItemOnPageCntInt / 2)) / NewItemsOnPageNum) + 1;
        //        }
        //        catch
        //        {
        //            NewPageNum = 1;
        //        }
        //    }
        //    return View("Index", GetItems4ListItemPage(NewPageNum, NewItemsOnPageNum));
        //}

        [HttpGet]
        public ActionResult Article(int id = 1)
        {
            var session = NHibernateHelper.OpenSession();
            var article = session.Get<Article>(id);
            return View(article);
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
