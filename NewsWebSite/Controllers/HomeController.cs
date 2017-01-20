using NewsWebSite.Models.ViewModel;
using NewsWebSite.Models;
using Newtonsoft.Json;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace NewsWebSite.Controllers
{
    public class HomeController : Controller
    {
        const int NumberOfItemsOnPage = 15;
        //const string UserImagesFolder = "~/Content/UserImages/";
        readonly IRepository repo;


        public HomeController(IRepository repo)
        {
            this.repo = repo;
        }



        //создает тестовые записи в таблице, для дебага
        [HttpGet]
        public ActionResult CreateLines(int n = 0)
        {
            var session = NHibernateHelper.OpenSession();
            for (int i = 1; i <= n; i++)
            {
                var a = new Article();
                a.Title = i.ToString();
                //a.CreateDate = DateTime.Now;
                //a.LastUpdateDate = DateTime.Now;
                repo.SaveOrUpdate(a);

            }
            return Content("ok");
        }
        //-------------------------------------------



        [HttpGet]
        public ActionResult Article(int id = 0)
        {
            if (id > 0)
            {
                var article = repo.GetItem(id);
                return View(article);
            }
            return HttpNotFound();
        }

        public ActionResult CreateArticle()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditArticle(int id = 0)
        {
            if (id < 1) return HttpNotFound();
            var article = repo.GetItem(id);
            if (article == null) return HttpNotFound();
            return View(new EditArticleModel(article));
        }

        [HttpPost]
        public ActionResult EditArticle(EditArticleModel edited)
        {
            if (!ModelState.IsValid) return View(edited);
            var baseArticle = repo.GetItem(edited.Id);
            if (baseArticle == null) return HttpNotFound();
            var changesExist = false;
            if (edited.Image != null)
            {
                FIleHelper.SaveOrUpdateArticleImage(Server.MapPath(ConfigurationManager.AppSettings["UserImagesFolder"].ToString()), edited.Image, baseArticle.Id);
                baseArticle.Image = edited.Image.FileName;
                changesExist = true;
            }
            if (edited.Title != null)
            {
                baseArticle.Title = edited.Title;
                changesExist = true;
            }
            if (edited.FullDescription != null)
            {
                baseArticle.FullDescription = edited.FullDescription;
                changesExist = true;
            }

            if (changesExist)
                repo.SaveOrUpdate(baseArticle);
            return RedirectToAction("Article", new { Id = edited.Id });
        }

        [HttpPost]
        public ActionResult CreateArticle(CreateArticleModel a)
        {
            if (!ModelState.IsValid) return View(a);
            var id = repo.SaveOrUpdate(a);
            FIleHelper.SaveOrUpdateArticleImage(Server.MapPath(ConfigurationManager.AppSettings["UserImagesFolder"].ToString()), a.Image, id);
            return RedirectToAction("Article", new { Id = id });
        }

        [HttpGet]
        public ActionResult Index()
        {
            var list = repo.GetDemoList(0, NumberOfItemsOnPage);
            var model = new ListItemPageModel(NumberOfItemsOnPage, list);
            return View(model);
        }

        [HttpPost]
        public ActionResult GetNewPageOfArticles(int Page)
        {
            if (Page < 1) return Content("");
            var list = repo.GetDemoListJson(Page * NumberOfItemsOnPage, NumberOfItemsOnPage);
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetNPagesOfArticles(int n)
        {
            var list = repo.GetDemoListJson(NumberOfItemsOnPage, n * NumberOfItemsOnPage);
            return Json(list);
        }
    }
}
