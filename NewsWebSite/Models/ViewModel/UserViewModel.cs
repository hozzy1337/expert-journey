using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.ViewModel
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public ISet<Tag> UserTags { get; set; }
        public IEnumerable<Tag> AllTags { get; set; }
    }
}