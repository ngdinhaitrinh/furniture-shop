using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanNoiThat.Models;
using WebBanNoiThat.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNetCoreHero.ToastNotification;

using AspNetCoreHero.ToastNotification.Extensions;
using NETCore.MailKit.Core;
using Microsoft.AspNetCore.Mvc;

namespace WebBanNoiThat
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

            services.AddTransient<MailService>();

            // Các cấu hình và dịch vụ khác...
            services.AddControllersWithViews();
            services.AddDbContext<BanNoiThatContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BanNoiThatConnect")));
            services.AddMvc()
            .AddSessionStateTempDataProvider();

            services.AddScoped<ILoaiSpRepository, DanhMucSpRepository>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });

           

            services.AddRazorPages();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options =>
                 {
                 options.Cookie.Name = "Banghang.Cookies";
                 options.ExpireTimeSpan = TimeSpan.FromHours( 8) ;
                 options.SlidingExpiration = true;
                 options.LoginPath = "/Admin/AdminAccount/Login";
                 options.AccessDeniedPath = "/Admin/AdminAccount/Login";
                 options.Cookie.HttpOnly = true;
                 options.Cookie.IsEssential = true;

 });
            services.Configure<IdentityOptions>(options => {
                // Thi?t l?p v? Password
                options.Password.RequireDigit = false; // Không b?t ph?i có s?
                options.Password.RequireLowercase = false; // Không b?t ph?i có ch? th??ng
                options.Password.RequireNonAlphanumeric = false; // Không b?t ký t? ??c bi?t
                options.Password.RequireUppercase = false; // Không b?t bu?c ch? in
                options.Password.RequiredLength = 6; // S? ký t? t?i thi?u c?a password


                // C?u hình Lockout - khóa user

                options.Lockout.MaxFailedAccessAttempts = 5; // Th?t b?i 5n l? thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // C?u hình v? User.
                options.User.AllowedUserNameCharacters = // các ký t? ??t tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nh?t


            });

         
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
         );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();


            });

        }
    }
}
