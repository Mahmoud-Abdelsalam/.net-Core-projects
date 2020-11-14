using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingSite.Models;
using ShoppingSite.Models.Interfaces;
using ShoppingSite.Models.SqlRepositories;

namespace ShoppingSite
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddScoped<IProductRepository, SqlProductRepository>();
            services.AddScoped<IOrderRepository, SqlOrderRepository>();
            services.AddScoped<ICartRepository, SqlCartRepository>();
            services.AddScoped<IReviewRepository, SqlReviewRepository>();
            services.AddDbContextPool<AppDbContext>(
                options => options.UseMySql(_config.GetConnectionString("ProductDBConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 5;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredUniqueChars = 3;
                    }
                )
                .AddEntityFrameworkStores<AppDbContext>();

            //Enable Cookie based authorization for MVC views

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddAuthorization(options =>
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin")
                ));

            //services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //});

            services.ConfigureApplicationCookie(options => { options.AccessDeniedPath = new PathString("/Administration/AccessDenied"); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            //Enable Cookie based authorization for MVC views
            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}