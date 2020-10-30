using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeleniumWeb.Models;
using SeleniumWeb.Services;

namespace SeleniumWeb
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
            //services.AddMvcCore();              // 核心服务,一般不太常用
            services.AddControllers();            // 纯WebAPI
            //services.AddRazorPages();           // AddMvcCore + Razor
            //services.AddControllersWithViews(); // 标准MVC
            //services.AddMvc();                  // AddControllersWithViews + Razor(全功能)

            // Mysql 连接
            services.AddDbContext<MyDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("Database")));

            // 依赖注入
            services.AddTransient<ISW0001Service, SW0001Service>();

            // 身份验证
            services.AddIdentity<LoginUser, IdentityRole>().AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

            // 认证
            services.AddAuthentication(options => {
                // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // JwtBearer认证
                // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // JwtBearer认证
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            // Spa静态文件根目录
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 添加MIME
            //var provider = new FileExtensionContentTypeProvider();
            //provider.Mappings[".script"] = "text/javascript";
            //provider.Mappings.Remove(".js");
            //app.UseStaticFiles(new StaticFileOptions() { ContentTypeProvider = provider });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseDefaultFiles();     // 启用默认网页
            app.UseHttpsRedirection();
            app.UseStaticFiles();        // 启用静态文件服务
            //app.UseDirectoryBrowser(); // 启动目录浏览
            app.UseSpaStaticFiles();     // 使用Spa静态文件服务

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });                          // 负载均衡重定向

            app.UseRouting();            // 路由

            app.UseAuthorization();      // 身份验证
            app.UseAuthentication();     // 认证

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            // 使用Spa网页
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
