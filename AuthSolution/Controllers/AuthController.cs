using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthSolution.Models;
using AuthSolution.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthSolution.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private IUserService userService;
        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: /<controller>/
        [Route("signin")]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new SignInModel());
        }

        [Route("signin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                User user;
                if (await userService.ValidateCredential(model.Username, model.Password, out user))
                {
                    await SignInUser(user.Username);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home"); 
                }
            }
            ModelState.AddModelError("Error", "Can't login");

            return View(model);

        }

        public async Task SignInUser(string username)
        {
            var claims = new List<Claim> {
                {new Claim(ClaimTypes.NameIdentifier, username)},
                {new Claim("name", username) }
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", null);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }

        [Route("signout")]
        [HttpPost]
        public async Task<ActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [Route("signup")]
        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new SignUpModel()); 
        }

        [Route("signup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(SignUpModel model)
        { 
            if (ModelState.IsValid)
            {
                if (await this.userService.Adduser(model.Username, model.Password))
                {
                    await SignInUser(model.Username);
                    return RedirectToAction("Index", "Home"); 
                }
                ModelState.AddModelError("Error", "Could not add user. Username already in use ...");
            }
            return View(model);
        }
    }
}
