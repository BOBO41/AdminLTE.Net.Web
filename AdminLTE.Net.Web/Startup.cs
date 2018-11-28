using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.Domain;
using AdminLTE.Models.AppSetting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AdminLTE.Domain.Service;

namespace AdminLTE.Net.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(option=>
            {
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            //������"XSRF-TOKEN"��ʶ,ֵΪ���Զ����ɵķ�α���
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddAuthentication(CookieService.AuthenticationScheme)
                    .AddCookie(CookieService.AuthenticationScheme, o =>
            {
                o.LoginPath = new PathString("/Login");
            });

            //����
            services.Configure<AppConfig>(Configuration);
            services.AddScoped<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            app.UseConfigRegist(Configuration);

            app.UseAuthentication();
        }
    }
}
