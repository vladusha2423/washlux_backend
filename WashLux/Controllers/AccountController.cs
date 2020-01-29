using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WashLux.Models;
using WashLux.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WashLux.Controllers
{
    [Authorize]
    [Route("api/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IJwtGenerator _jwt;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager/*, IJwtGenerator jwt*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_jwt = jwt;
        }

        
        [HttpGet]
        public ActionResult<List<User>> Users()
        {
            return _userManager.Users.ToList();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register([FromBody]AuthModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Login };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Ok(JwtGenerator.GenerateJwt(_userManager.Users.FirstOrDefault(u => u.UserName == model.Login)));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Login([FromBody]AuthModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok(JwtGenerator.GenerateJwt(await _userManager.FindByNameAsync(model.Login)));
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return BadRequest(ModelState);
        }



        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        
        [HttpGet]
        public ActionResult<bool> isAuth()
        {
            var sender = HttpContext.User.Identity.Name;
            return Ok(_userManager.Users.Any(u => u.UserName == sender));
        }

    }
}
