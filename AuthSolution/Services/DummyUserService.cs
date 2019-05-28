using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AuthSolution.Services
{
    public class DummyUserService: IUserService
    {

        private IDictionary<string, (string PasswordHash, User User)> _users
            = new Dictionary<string, (string PasswordHash, User User)>();

        public DummyUserService(IDictionary<string, string> users)
        {
            foreach(var user in users)
            {
                _users.Add(user.Key.ToLower(), (BCrypt.Net.BCrypt.HashPassword(user.Value), new User(user.Key))); 
            }
        }

        public Task<bool> Adduser(string username, string password)
        {
            if (this._users.ContainsKey(username.ToLower()))
            {
                return Task.FromResult(false); 
            }

            _users.Add(username.ToLower(), (BCrypt.Net.BCrypt.HashPassword(password), new User(username.ToLower())));
            return Task.FromResult(true);
        }

        public Task<bool> ValidateCredential(string username, string password, out User user)
        {
            user = null;
            var key = username.ToLower();
            if (_users.Where(n => n.Key.Contains(key)).Count() > 0)
            {
                var hash = _users[key].PasswordHash;
                if(BCrypt.Net.BCrypt.Verify(password,hash))
                {
                    user = _users[key].User;
                    return Task.FromResult(true);
                }
            }   

            return Task.FromResult(false);
        }
    }
}
