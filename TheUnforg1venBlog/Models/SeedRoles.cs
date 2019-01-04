using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TheUnforg1venBlog.Models
{
	public class SeedRoles
	{
		public static async Task CreateRoles(IServiceProvider serviceProvider)
		{
			// create roleManager service
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			// create userManager servicew
			var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

			// array with roles
			string[] roleNames = { "Admin", "Moderator" };

			// result variable
			IdentityResult roleResult;

			// add roles using RoleManager
			foreach (var roleName in roleNames)
			{
				// check if current role exist
				var roleExist = await RoleManager.RoleExistsAsync(roleName);

				// if not - create new role
				if (!roleExist)
					roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
			}

			/*  ---   Admin user creating   ---  */

			// create admin user
			var userAdmin = await UserManager.FindByNameAsync("Admin");

			// if there no 'admin'
			if (userAdmin == null)
			{
				// create new admin
				userAdmin = new IdentityUser()
				{
					UserName = "Admin",
					Email = "admin@gmail.com",
				};

				// creates identity structure with login and password
				await UserManager.CreateAsync(userAdmin, "Secret123");
			}

			// add new user into 'Admin' role
			await UserManager.AddToRoleAsync(userAdmin, "Admin");

			/*  ---   Moderator user creating   ---  */

			// create moderator user
			var userModerator = await UserManager.FindByNameAsync("Moderator1");

			// if there are no moderator
			if (userModerator == null)
			{
				// create new admin moderator
				userModerator = new IdentityUser()
				{
					UserName = "Moderator1",
					Email = "moderator@gmail.com",
				};

				// creates identity structure with login and password
				await UserManager.CreateAsync(userModerator, "Moder12345");
			}

			// add moderator into 'Moderator' role
			await UserManager.AddToRoleAsync(userModerator, "Moderator");
		}
	}
}
