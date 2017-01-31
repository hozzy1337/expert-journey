using NewsWebSite.Controllers;
using NewsWebSite.Models.ViewModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public interface IArticleRepository
    {
        Article GetItem(int id);
        int GetCountOfLines();
        PagedList<DemoArticle> GetDemoList(int starFrom, int count, int lastId, string[] tagList);
        int Save(Article article);
        bool IsExist(int id);
    }

    /* public class CachedRepository : IRepository
     {
         readonly IRepository _realRepo;

         public CachedRepository(IRepository realRepo)
         {
             _realRepo = realRepo;
         }

         public Article GetItem(int id)
         {

             //check if article exsit in cache
             var article = _realRepo.GetItem(id);

             var cacheKey = string.Join("::", id, article.LastUpdateDate);
             //put to cache
         }

         public PagedList<Article> GetList(int offset = 0, int count = 10)
         {
             throw new NotImplementedException();
         }

         public void Save(Article article)
         {
             //remove from cache by key
         }
     } */

    public class PagedList<T> : List<T>
    {
        public int LinesCount { get; set; }
        public int PageCount { get; set; }
    }
}