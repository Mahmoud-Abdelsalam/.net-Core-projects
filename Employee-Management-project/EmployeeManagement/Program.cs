using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EmployeeManagement
{
    public class Program
    {
        //This Main() method configures asp.net core and starts it and at that
        //point it becomes an asp.net core web application.
        //   asp.net core application initially starts as a console application and
        // the Main() method in Program.cs file is the entry point.
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder() method returns an object that implements IWebHostBuilder.
            //Build() method is called which builds a web host that hosts our asp.net core web application.
            //Run() method is called, which runs the web application and
            //it begins listening for incoming HTTP requests.
            CreateWebHostBuilder(args).Build().Run();
        }
        //CreateWebHostBuilder() method calls CreateDefaultBuilder() static method of the WebHost class.
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddEventSourceLogger();
                        logging.AddNLog();
                        
                    })
                         //Startup class is also configured using the UseStartup()
                         //extension method of IWebHostBuilder class.
                         .UseStartup<Startup>();
    }
}
