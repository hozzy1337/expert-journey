using NewsWebSite.Attributes;
using NewsWebSite.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Models
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string FullDescription { get; set; }
        public virtual string Image { get; set; }
        public virtual string CreateDate { get; set; }
        public virtual string LastUpdateDate { get; set; }

        public Article(EditArticleModel a)
        {
            Title = a.Title;
            FullDescription = a.FullDescription;
        }

        public Article()
        {
        }
    }
}