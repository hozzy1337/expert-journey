using Microsoft.AspNet.Identity;
using NewsWebSite.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NewsWebSite.Models
{
    public class CustomUserStore : IUserStore<User>
    {
        readonly IUserRepository repository;
        public CustomUserStore(IUserRepository repository)
        {
            this.repository = repository;
        }

        public Task CreateAsync(User user)
        {
            return Task.FromResult(repository.Save(user));
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return Task.FromResult(repository.GetById(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.FromResult(repository.FindByName(userName));
        }

        public Task UpdateAsync(User user)
        {
            return Task.FromResult(repository.Save(user));
        }
    }
}