﻿using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public class NHibernateRepository : IRepository
    {
        readonly ISessionFactory sessionFactory;

        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public int Save(Article a)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var t = session.BeginTransaction())
                {
                    var timeNow = DateTime.Now;
                    if (a.CreateDate == DateTime.MinValue)
                        a.CreateDate = timeNow;
                    a.LastUpdateDate = timeNow;
                    session.SaveOrUpdate(a);
                    t.Commit();
                    return a.Id;
                }
            }
        }

        public Article GetItem(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Article>(id);
            }
        }

        #region NotUsedNow
        /* public PagedList<Article> GetList(int starFrom = 0, int count = 10)
         {
             using (var session = sessionFactory.OpenSession())
             {
                 var results = new PagedList<Article>();
                 results.AddRange(session.CreateCriteria<Article>()
                 .SetFirstResult(starFrom)
                 .SetMaxResults(count)
                 .AddOrder(Order.Desc("Id"))
                 .List<Article>());
                 results.LinesCount = session.QueryOver<Article>()
                     .Select(Projections.RowCount())
                     .FutureValue<int>()
                     .Value;
                 results.PageCount = (int)Math.Ceiling(results.LinesCount / double.Parse(System.Configuration.ConfigurationManager.AppSettings["NumberOfItemsOnPage"]));
                 return results;
             }
         } */
        #endregion

        public PagedList<DemoArticle> GetDemoList(int starFrom, int count, int lastId, string[] taglist, int userId)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var creteria = session.CreateCriteria<Article>()
                    .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Id(), "Id")
                    .Add(Projections.Property("Title"), "Title")
                    .Add(Projections.Property("Image"), "Image")
                    .Add(Projections.Property("CreateDate"), "CreateDate")
                    .Add(Projections.Property("LastUpdateDate"), "LastUpdateDate"));

                if (userId > 0)
                {
                    creteria.Add(Restrictions.Eq("UserId", userId));
                }
                if (taglist != null)
                {
                    foreach (var t in taglist)
                    {
                        if (t != "") creteria.Add(Restrictions.Like("Tags", "," + t + ",", MatchMode.Anywhere));
                    }
                }
                if (lastId > 0) creteria.Add(Restrictions.Lt("Id", lastId));
                else creteria.SetFirstResult(starFrom);

                var results = new PagedList<DemoArticle>();

                results.AddRange(creteria
                    .AddOrder(Order.Desc("Id"))
                    .SetMaxResults(count)
                    .SetResultTransformer(Transformers.AliasToBean<DemoArticle>())
                    .List<DemoArticle>());

                results.LinesCount = session.QueryOver<Article>()
                    .Select(Projections.RowCount())
                    .FutureValue<int>()
                    .Value;

                results.PageCount = (int)Math.Ceiling(results.LinesCount / double.Parse(System.Configuration.ConfigurationManager.AppSettings["NumberOfItemsOnPage"]));
                return results;
            }
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

    public class NewsCriteria
    {
        public int StartFrom { get; set; }
        public int Count { get; set; }
        public int LastId { get; set; }
        public string[] Tags { get; set; }
        public int UserId { get; set; }

        public NewsCriteria()
        {
            Tags = new string[0];
            Count = 10;
        }
    }
}