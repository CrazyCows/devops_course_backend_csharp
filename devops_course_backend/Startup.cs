using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using devops_course_backend.dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using devops_course_backend.services;

namespace devops_course_backend
{

    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<UserContext>(options =>
                options.UseNpgsql("Host=localhost;Database=mydb;Username=myuser;Password=mypassword"));


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options => {
                options.Cookie.IsEssential = true;
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "260729048541-br4tr166p0fsj1mhenohfhis2h5870r0.apps.googleusercontent.com";
                googleOptions.ClientSecret = "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";
                googleOptions.CallbackPath = "/signin-google";
            });

            services.AddHttpContextAccessor();
            services.AddScoped<CustomerService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
