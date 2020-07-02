using System;
using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration _config;
        //Startup class does the following 2 very important things
        //ConfigureServices() method configures services required by the application
        // Configure() method sets up the application's request processing pipeline
        public Startup(IConfiguration config)
        {
            _config = config;
        }
         
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => 
                options.UseSqlServer(_config.GetConnectionString("EmployeeDbConnection")));
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser().
                    Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "505299103742-en63sja6d7og49tq1hqum1rjj547rgat.apps.googleusercontent.com";
                    options.ClientSecret = "Vv_8OnrnZkiRAAor3KvaQUmZ";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "2388458421453953";
                    options.AppSecret = "1cebc9c2fd0ed0703499c6d2551e2db8";
                });
                  
            services.ConfigureApplicationCookie(option =>
                                                                          option.AccessDeniedPath = new PathString("/Administration/AccessDenied"));
            services.AddAuthorization(options =>
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true")
                                                                                                                                       
                ));  
            services.AddAuthorization(options =>
                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRulesAndClaimsRequirement())
                                                                                                                                       
                ));  
            services.AddAuthorization(options =>
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "true")
                                                                                                                                       
                ));
            services.AddAuthorization(options =>
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin" , "Super Admin")
                ));
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminsRulesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 5;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredUniqueChars = 3;

                        options.SignIn.RequireConfirmedEmail = true;
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(15);
                    }
                )
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            //THIS METHOD IS REQUIRED TO
            //ALLOW CHANGING THE LIFE TIME OF ALL TOKEN PROVIDERS (EMAIL CONFIRMATION , RESET PASSWORD )LIFE TIME
            //(IT IS 1 DAY BY DEFAULT)
            //services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(5));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env  )
        {
            
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
