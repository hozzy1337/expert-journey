using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.SignalrModels
{
    public class CommentInfo
    {
        public int Depth { get; set; }
        public int ArticleId { get; set; }
    }
}