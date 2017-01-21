﻿using NewsWebSite.Attributes;
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
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime LastUpdateDate { get; set; }

        public Article(CreateArticleModel model)
        {
            Title = model.Title;
            FullDescription = model.FullDescription;
            Image = model.Image.FileName;
        }
        public Article()
        {
            Title = null;
            FullDescription = null;
            Image = null;
            CreateDate = DateTime.MinValue;
            LastUpdateDate = DateTime.MinValue;
        }
    }
}