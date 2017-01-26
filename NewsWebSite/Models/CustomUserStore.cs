using Microsoft.AspNet.Identity;
using NewsWebSite.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NewsWebSite.Models
{
    public class CustomUserStore : IUserStore<User, int>, IUserPasswordStore<User, int>, IUserLockoutStore<User, int>
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
            return Task.FromResult(true);
        }

        public void Dispose()
        {
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.FromResult(repository.GetById(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.FromResult(repository.FindByName(userName));
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(user.lockoutEnd);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(string.IsNullOrEmpty(user.Password));
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(--user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(true);

        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(true);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.lockoutEnd = lockoutEnd;
            return Task.FromResult(true);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(true);
        }

        public Task UpdateAsync(User user)
        {
            return Task.FromResult(repository.Save(user));
        }
    }
}