using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        readonly ISessionFactory sessionFactory;

        public ArticleRepository(ISessionFactory sessionFactory)
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

        public bool IsExist(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var count = session.CreateCriteria<Article>()
                    .SetProjection(Projections.RowCount())
                    .Add(Restrictions.IdEq(id))
                    .UniqueResult<int>();
                return count == 1;
            }
        }

        public Article GetItem(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Article>(id);
            }
        }

   

        public PagedList<DemoArticle> GetDemoList(int starFrom, int count, int lastId, string[] taglist/*, int userId*/)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var filter = session.CreateCriteria<Article>();
                   

                //if (userId > 0)
                //{
                //    creteria.Add(Restrictions.Eq("UserId", userId));
                //}
                if (taglist != null)
                {
                    foreach (var t in taglist)
                    {
                        //if (t != "") creteria.Add(Restrictions.Like("Tags", "," + t + ",", MatchMode.Anywhere)); 
                        //Vadim 
                    }
                }
                if (lastId > 0) filter.Add(Restrictions.Lt("Id", lastId));
                else filter.SetFirstResult(starFrom);

                var results = new PagedList<DemoArticle>();
                var countCreteria = (ICriteria)filter.Clone();
                results.AddRange(filter
                     .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Id(), "Id")
                    .Add(Projections.Property("Title"), "Title")
                    .Add(Projections.Property("Image"), "Image")
                    .Add(Projections.Property("CreateDate"), "CreateDate")
                    .Add(Projections.Property("LastUpdateDate"), "LastUpdateDate"))
                    .AddOrder(Order.Desc("Id"))
                    .SetMaxResults(count)
                    .SetResultTransformer(Transformers.AliasToBean<DemoArticle>())
                    .List<DemoArticle>());

                results.LinesCount = countCreteria.SetProjection(Projections.RowCount()).UniqueResult<int>();

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