using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AuthController : Controller
    {
        private readonly UserManager<WorldUser> _userManager;
        private readonly SignInManager<WorldUser> _signInManager;

        private readonly IMapper _mapper;
        public AuthController(UserManager<WorldUser> userManager,
        SignInManager<WorldUser> signInManager, IMapper mapper)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Trips", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(loginVm.UserName, loginVm.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Trips", "App");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Username or password is invalid");
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel regiVm)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(regiVm.Email) == null)
                {
                    var newUser = _mapper.Map<WorldUser>(regiVm);
                    var userForReturn = _mapper.Map<RegisterViewModel>(newUser);
                    var result = await _userManager.CreateAsync(newUser, regiVm.Password);
                    if (result.Succeeded)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(regiVm.UserName, regiVm.Password, true, false);
                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Trips", "App");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email already exist");
                }
            }

            return View();
        }
    }
}