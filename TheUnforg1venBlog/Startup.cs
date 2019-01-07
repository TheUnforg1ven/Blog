using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheUnforg1venBlog.Data;
using TheUnforg1venBlog.Data.Interfaces;
using TheUnforg1venBlog.Services.FileManager;

namespace TheUnforg1venBlog
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IHostingEnvironment hostingEnvironment)
		{
			_configuration = new ConfigurationBuilder()
				.SetBasePath(hostingEnvironment.ContentRootPath)
				.AddJsonFile("appsettings.json")
				.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// add Db context with default connection
			services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

			// add identity context 
			services.AddIdentity<IdentityUser, IdentityRole>(options => 
			{
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 8;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders(); 

			services.AddTransient<IPostRepository, PostRepository>();
			services.AddTransient<IFileManager, FileManager>();

			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
								ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				loggerFactory.AddConsole();
				app.UseStatusCodePages();
				app.UseStaticFiles();
				app.UseAuthentication();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			// add all routes
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "Error",
					template: "Error",
					defaults: new { controller = "Error", action = "Error" }
				);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
			});

			//SeedRoles.CreateRoles(serviceProvider).Wait();
		}
	}
}
