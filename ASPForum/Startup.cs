using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace ASPForum
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().WithRazorPagesRoot("/Pages");
			//services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.AccessDeniedPath = "/Index.cshtml";
					options.Cookie.Name = AdminClasses.AdminManager.LoginCookie;
					//options.LoginPath = "";
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//if (env.IsDevelopment())
			//{
			//	app.UseDeveloperExceptionPage();
			//}

			//app.UseDeveloperExceptionPage();
			app.UseExceptionHandler("/Error");

			app.UseFileServer();
			app.UseAuthentication();
			app.UseMvc();

			/*app.Run(async (context) =>
			{
				await context.Response.WriteAsync("Goodbye World!");
			});*/


		}
	}
}
