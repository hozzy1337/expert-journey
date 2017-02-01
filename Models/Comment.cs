﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class Comment
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Text { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual int Depth { get; set; }
        public virtual int BaseCommentId { get; set; }
        public virtual DateTime Created { get; set; }
    }
}