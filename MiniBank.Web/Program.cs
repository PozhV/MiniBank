using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace MiniBank.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    try
                    {
                        
                        webBuilder.UseStartup<Startup>();
                    }
                    catch (Exception ex)
                    {
                        var stack = new StackTrace(ex);
                        foreach (StackFrame frame in stack.GetFrames())
                        {
                            Console.WriteLine(frame.GetMethod());
                        }
                    }
                     });
    }
}

