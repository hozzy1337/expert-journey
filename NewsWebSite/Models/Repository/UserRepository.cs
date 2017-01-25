using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        readonly ISessionFactory sessionFactory;

        public UserRepository(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public string Save(User u)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(u);
                    transaction.Commit();
                    return u.Id;
                }
            }
        }
        public User GetById(string Id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<User>(Id);
            }
        }
        public User FindByName(string name)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var user = session.CreateCriteria<User>().Add(Restrictions.Eq("UserName", name)).List<User>()[0];
                return user;
                // return session.Get<User>(Id);
            }
        }
    }
}