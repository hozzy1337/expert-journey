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
        IList<Article> GetList(int starfrom = 0, int count = 10);
        IList<DemoArticle> GetDemoList(int starfrom = 0, int count = 10);
        void Save(Article article);
    }

    public class NHibernateRepository : IRepository
    {
        readonly ISessionFactory sessionFactory;

        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Save(Article a)
        {
            using (var session = sessionFactory.OpenSession())
            {
                session.BeginTransaction();
                session.Save(a);
                session.Transaction.Commit();
            }
        }
        public Article GetItem(int id)
        {
            Article article;
            using (var session = sessionFactory.OpenSession())
            {
                article = session.Get<Article>(id);
            }
            return article;
        }

        public IList<Article> GetList(int starfrom = 0, int count = 10)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var list = session.CreateCriteria<Article>()
                    .SetFirstResult(starfrom)
                    .SetMaxResults(count)
                    .AddOrder(Order.Desc("Id"))
                    .List<Article>();//достаем записи
                return list;
            }

        }
        public IList<DemoArticle> GetDemoList(int starfrom = 0, int count = 10)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var results = session.CreateCriteria<Article>()
                    .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Id(), "Id")
                    .Add(Projections.Property("Title"), "Title")
                    .Add(Projections.Property("Image"), "Image")
                    .Add(Projections.Property("CreateDate"), "CreateDate")
                    .Add(Projections.Property("LastUpdateDate"), "LastUpdateDate"))
                    .SetFirstResult(starfrom)
                    .SetMaxResults(count)
                    .AddOrder(Order.Desc("Id"))
                    .SetResultTransformer(Transformers.AliasToBean<DemoArticle>())
                    .List<DemoArticle>();
                //var list = session.CreateCriteria<Article>().SetFirstResult(starfrom).SetMaxResults(count).AddOrder(Order.Desc("Id")).List<Article>();//достаем записи
                return results;
            }
        }
        public int GetCountOfLines()
        {
            using (var session = sessionFactory.OpenSession())
            {
                var count = session.QueryOver<Article>().Select(Projections.RowCount()).FutureValue<int>().Value; //достаем количество записей в таблице
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