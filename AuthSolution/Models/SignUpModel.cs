using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSolution.Models
{
    public class SignUpModel
    {
        public SignUpModel()
        {
        }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}
