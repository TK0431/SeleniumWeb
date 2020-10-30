using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SeleniumWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel(); // 内置Web服务器
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory()); // 根文件夹位置
                    webBuilder.UseIISIntegration(); // IIS反向代理
                    webBuilder.UseStartup<Startup>(); // 初始化类
                });
    }
}
