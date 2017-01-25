using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public interface IUserRepository
    {
        string Save(User u);
//        void Update(User u);
        User GetById(string id);
        User FindByName(string name);

    }
}