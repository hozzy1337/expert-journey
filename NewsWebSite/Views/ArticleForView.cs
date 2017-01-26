using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class ArticleForView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FullDescription { get; set; }
        public string Image { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string Tags { get; set; }
        public bool Editable { get; set; }

        public ArticleForView(Article a)
        {
            Id = a.Id;
            Title = a.Title;
            FullDescription = a.FullDescription;
            Image = a.Image;
            Tags = (new TagsHelper()).GetLineToShow(a.Tags);
            CreateDate = a.CreateDate.ToString("R");
            UpdateDate = (a.CreateDate != a.LastUpdateDate ? a.LastUpdateDate.ToString("R") : null);
            Editable = false;
        }
        public ArticleForView() { }
    }
    
}