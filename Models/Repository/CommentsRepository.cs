using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsWebSite.Models.SignalrModels;
using NHibernate.Transform;

namespace NewsWebSite.Models.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        readonly ISessionFactory sessionFactory;
        public CommentsRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }



        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int GetArticleId(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var articleId = session.CreateCriteria<Comment>()
                .SetProjection(Projections.Property("ArticleId"))
                .Add(Restrictions.IdEq(id))
                .UniqueResult<int>();
                return articleId;
            }
        }

        public int GetBaseCommentDepth(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var baseCommentDepth = session.CreateCriteria<Comment>()
                .SetProjection(Projections.Property("Depth"))
                .Add(Restrictions.IdEq(id))
                .UniqueResult<int>();
                return baseCommentDepth;
            }
        }

        public int GetBaseCommentId(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var baseCommentId = session.CreateCriteria<Comment>()
                .SetProjection(Projections.Property("BaseCommentId"))
                .Add(Restrictions.IdEq(id))
                .UniqueResult<int>();
                return baseCommentId;
            }
        }

        public CommentInfo GetCommentInfo(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var baseCommentId = session.CreateCriteria<Comment>()
                .SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("Depth"), "Depth")
                .Add(Projections.Property("BaseArticleId"), "BaseArticleId"))
                .Add(Restrictions.IdEq(id))
                .SetResultTransformer(Transformers.AliasToBean<CommentInfo>())
                .UniqueResult<CommentInfo>();

              
                return baseCommentId;
            }
        }

        public Comment GetItem(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Comment>(id);
            }
        }

        public PagedList<Comment> GetList(int articleId)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var count = session.CreateCriteria<Comment>()
                .SetProjection(Projections.RowCount())
                .Add(Restrictions.IdEq(id))
                .UniqueResult<int>();
                return count == 1;
            }
        }

        public int Save(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}