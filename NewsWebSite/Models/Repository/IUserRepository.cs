using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public interface IUserRepository
    {
        int Save(User u);
//        void Update(User u);
        User GetById(int id);
        User FindByName(string name);

    }
}