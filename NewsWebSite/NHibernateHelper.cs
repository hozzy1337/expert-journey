using NewsWebSite.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            //todo: remove to web.config
            var configurePath = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\Nhibernate.cfg.xml");
            configuration.Configure(configurePath);
            //Если бы не сделали Book.hbm.xml Embedded Resource, то он бы написал ошибку о невозможности найти файл
            configuration.AddAssembly(typeof(Article).Assembly);


            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            //Позволяет Nhibernate самому создавать в БД таблицу и поля к ним. 
           // new SchemaExport(configuration).Execute()
            new SchemaUpdate(configuration).Execute(true, true);
            return sessionFactory.OpenSession();
        }
    }
}