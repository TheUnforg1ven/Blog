using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TheUnforg1venBlog.ViewModels;

namespace TheUnforg1venBlog.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;

		private readonly SignInManager<IdentityUser> _signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel
			{
				ReturnUrl = returnUrl
			});
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			// if modelstate unvalid - return same view
			if (!ModelState.IsValid)
				return View(loginViewModel);

			var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

			// if user exists
			if (user != null)
			{
				// check login and password
				var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

				// all succeeded
				if (result.Succeeded)
				{
					// if empty return url
					if (String.IsNullOrEmpty(loginViewModel.ReturnUrl))
						// go to home page
						return RedirectToAction("Index", "Home");

					// redirect bu return url
					return Redirect(loginViewModel.ReturnUrl);
				}
			}

			// if user not found
			ModelState.AddModelError(String.Empty, "Username or password was not found");

			// same view
			return View(loginViewModel);
		}

		/// <summary>
		/// Register method
		/// </summary>
		/// <returns>Register view</returns>
		public IActionResult Register() => View();

		/// <summary>
		/// Register post method
		/// </summary>
		/// <param name="loginViewModel">created viewmodel</param>
		/// <returns>same view if not ok</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(LoginViewModel loginViewModel)
		{
			// if state is valid
			if (ModelState.IsValid)
			{
				// craete new user
				var user = new IdentityUser() { UserName = loginViewModel.UserName };

				// create new account (needs new user)
				var result = await _userManager.CreateAsync(user, loginViewModel.Password);

				// if all is ok
				if (result.Succeeded)
				{
					// go to home page
					return RedirectToAction("Index", "Home");
				}

				// if not
				else
				{
					// add model errors
					foreach (var err in result.Errors)
						ModelState.AddModelError("", $"{err.Description}");
				}

			}

			// same view
			return View(loginViewModel);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			// simple sign out
			await _signInManager.SignOutAsync();

			// go to home page
			return RedirectToAction("Index", "Home");
		}
	}
}
