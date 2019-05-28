using System;
using System.Threading.Tasks;

namespace AuthSolution.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredential(string username, string password, out User user);
        Task<bool> Adduser(string username, string password);
    }


    public class User
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
