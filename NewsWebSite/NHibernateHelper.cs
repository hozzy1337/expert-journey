using NewsWebSite.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewsWebSite
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            var configuration = new NHibernate.Cfg.Configuration();
            //todo: remove to web.config
            var configurePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["NHibernateConfig"].ToString());
            configuration.Configure(configurePath);
            configuration.AddAssembly(typeof(Article).Assembly);


            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            new SchemaUpdate(configuration).Execute(true, true);
            return sessionFactory.OpenSession();
        }
    }
}