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
        public string DateMsg { get; set; }
      //  public string LastUpdateDate { get; set; }

        public ArticleForView(Article a)
        {
            Id = a.Id;
            Title = a.Title;
            FullDescription = a.FullDescription;
            Image = a.Image;
            DateMsg = "Created: " + a.CreateDate.ToString("MM/dd/yy H:mm") + (a.CreateDate != a.LastUpdateDate ? "<br/>Updated: " + a.LastUpdateDate.ToString("MM/dd/yy H:mm") : "");
        }
        public ArticleForView() { }
    }
    
}