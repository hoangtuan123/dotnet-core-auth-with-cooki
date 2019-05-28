using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSolution.Models
{
    public class SignInModel
    {
        public SignInModel()
        {
        }

        [Required(ErrorMessage = "Have to supply a username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Have to supply a password")]
        public string Password { get; set; }
    }
}
