using NewsWebSite.Models.ViewModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public interface IRepository
    {
        Article GetItem(int id);
        int GetCountOfLines();
        IList<Article> GetList(int starFrom, int count);
        PagedList<DemoArticle> GetDemoList(int starFrom, int count);
        int SaveOrUpdate(Article article);
        int SaveOrUpdate(CreateArticleModel model);
        string GetDemoListJson(int startFrom, int count);
    }

    public class NHibernateRepository : IRepository
    {
        readonly ISessionFactory sessionFactory;


        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }
      

        public int SaveOrUpdate(Article a)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var t = session.BeginTransaction())
                {
                    var timeNow = DateTime.Now.ToString("MM/dd/yy H:mm"); ;
                    if (a.CreateDate == null)
                        a.CreateDate = timeNow;
                    a.LastUpdateDate = timeNow;
                    session.SaveOrUpdate(a);
                    t.Commit();
                    return a.Id;

                }
            }
        }

       
        public int SaveOrUpdate(CreateArticleModel model)
        {
            Article a = new Article();
            a.Title = model.Title;
            a.FullDescription = model.FullDescription;
            a.Image = model.Image.FileName;
            return SaveOrUpdate(a);
        }


        public Article GetItem(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Article>(id);
            }
        }


        public IList<Article> GetList(int starFrom = 0, int count = 10)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var list = session.CreateCriteria<Article>()
                    .SetFirstResult(starFrom)
                    .SetMaxResults(count)
                    .AddOrder(Order.Desc("Id"))
                    .List<Article>();
                return list;
            }
        }


        public PagedList<DemoArticle> GetDemoList(int starFrom = 0, int count = 10)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var results = new PagedList<DemoArticle>();

                results.AddRange(session.CreateCriteria<Article>()
                    .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Id(), "Id")
                    .Add(Projections.Property("Title"), "Title")
                    .Add(Projections.Property("Image"), "Image")
                    .Add(Projections.Property("CreateDate"), "CreateDate")
                    .Add(Projections.Property("LastUpdateDate"), "LastUpdateDate"))
                    .SetFirstResult(starFrom)
                    .SetMaxResults(count)
                    .AddOrder(Order.Desc("Id"))
                    .SetResultTransformer(Transformers.AliasToBean<DemoArticle>())
                    .List<DemoArticle>());
                results.TotalCount = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
                return results;
            }
        }

        public string GetDemoListJson(int startFrom, int count)
        {
            var list = GetDemoList(startFrom, count);
            return JsonConvert.SerializeObject(list);
        }

        public int GetCountOfLines()
        {
            using (var session = sessionFactory.OpenSession())
            {
                var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value;
                return count;
            }
        }
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
        public int TotalCount { get; set; }
    }
}