using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories;
using eCommerceForSale.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using eCommerceForSale.Utility;
using System;
using Microsoft.AspNetCore.Routing;
using Stripe;

namespace eCommerceForSale.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.Configure<EmailOptions>(Configuration);
            services.Configure<MasterDataOptions>(Configuration);
            services.Configure<StripeOptions>(Configuration.GetSection("Stripe"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            services.AddAuthentication().AddFacebook(option =>
            {
                option.AppId = "236508214134926";
                option.AppSecret = "e41f3ecd90ebe06ef7ce797a56f866af";
            });
            services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientId = "255362192942-0f46uml25k4i61a6la0ivra60ps7grto.apps.googleusercontent.com";
                option.ClientSecret = "DfyIunhEmXXD3xhxZhoP6jzV";
            });

            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(30);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseStatusCodePagesWithRedirects("/Customer/Home/Error/{0}");
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Customer/Home/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["Secretkey"];

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

                CreateCustomeRouting(endpoints);
                //endpoints.MapControllerRoute(
                //   name: "product",
                //   pattern: "products/{id}",
                //   defaults: new { area = "Customer", controller = "Home", action = "Product" }
                //   );

                endpoints.MapRazorPages();
            });
        }

        private void CreateCustomeRouting(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                    name: "product",
                    pattern: "products/{id}",
                    defaults: new { area = "Customer", controller = "Home", action = "Product" }
                    );
            endpoints.MapControllerRoute(
                   name: "cart",
                   pattern: "user/shopping-cart",
                   defaults: new { area = "Customer", controller = "ShoppingCart", action = "Index" }
                   );
        }
    }
}