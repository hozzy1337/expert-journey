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

namespace NewsWebSite.Models
{
    public interface IRepository
    {
        Article GetItem(int id);
        int GetCountOfLines();
        PagedList<Article> GetList(int starFrom, int count);
        PagedList<DemoArticle> GetDemoList(int starFrom, int count);
        int Save(Article article);
    }

    //public class NHibernateRepository : IRepository
    //{
    //    readonly ISessionFactory sessionFactory;


    //    public NHibernateRepository(ISessionFactory sessionFactory)
    //    {
    //        this.sessionFactory = sessionFactory;
    //    }
      

    //    public int Save(Article a)
    //    {
    //        using (var session = sessionFactory.OpenSession())
    //        {
    //            using (var t = session.BeginTransaction())
    //            {
    //                var timeNow = DateTime.Now;
    //                if (a.CreateDate == DateTime.MinValue)
    //                    a.CreateDate = timeNow;
    //                a.LastUpdateDate = timeNow;
    //                session.SaveOrUpdate(a);
    //                t.Commit();
    //                return a.Id;

    //            }
    //        }
    //    }


    //    public Article GetItem(int id)
    //    {
    //        using (var session = sessionFactory.OpenSession())
    //        {
    //            return session.Get<Article>(id);
    //        }
    //    }


    //    public PagedList<Article> GetList(int starFrom = 0, int count = 10)
    //    {
    //        using (var session = sessionFactory.OpenSession())
    //        {
    //            var results = new PagedList<Article>();
    //                results.AddRange(session.CreateCriteria<Article>()
    //                .SetFirstResult(starFrom)
    //                .SetMaxResults(count)
    //                .AddOrder(Order.Desc("Id"))
    //                .List<Article>());
    //            results.LinesCount = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
    //            results.PageCount = (int)Math.Ceiling(results.LinesCount / double.Parse(ConfigurationManager.AppSettings["NumberOfItemsOnPage"]));
    //            return results;
    //        }
    //    }


    //    public PagedList<DemoArticle> GetDemoList(int starFrom = 0, int count = 10)
    //    {
    //        using (var session = sessionFactory.OpenSession())
    //        {
    //            var results = new PagedList<DemoArticle>();

    //            results.AddRange(session.CreateCriteria<Article>()
    //                .SetProjection(Projections.ProjectionList()
    //                .Add(Projections.Id(), "Id")
    //                .Add(Projections.Property("Title"), "Title")
    //                .Add(Projections.Property("Image"), "Image")
    //                .Add(Projections.Property("CreateDate"), "CreateDate")
    //                .Add(Projections.Property("LastUpdateDate"), "LastUpdateDate"))
    //                .SetFirstResult(starFrom)
    //                .SetMaxResults(count)
    //                .AddOrder(Order.Desc("Id"))
    //                .SetResultTransformer(Transformers.AliasToBean<DemoArticle>())
    //                .List<DemoArticle>());
    //            results.LinesCount = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
    //            results.PageCount = (int)Math.Ceiling(results.LinesCount / double.Parse(ConfigurationManager.AppSettings["NumberOfItemsOnPage"]));
    //            return results;
    //        }
    //    }


    //    public int GetCountOfLines()
    //    {
    //        using (var session = sessionFactory.OpenSession())
    //        {
    //            var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
    //            return count;
    //        }
    //    }
    //}
    
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