using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class Tag
    {
        private ISet<Article> articles = new HashSet<Article>();
        private ISet<AppUser> users = new HashSet<AppUser>();
        public virtual int Id { get; set; }
        public virtual string TagText { get; set; }
        public virtual ISet<Article> Articles
        {
            get
            {
                return articles;
            }
            set
            {
                articles = value;
            }
        }
        public virtual ISet<AppUser> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
            }
        }
    }
}