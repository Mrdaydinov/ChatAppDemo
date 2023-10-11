using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ChatAppDemo;

public class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }



    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBulder =>
                webBulder.UseStartup<Startup>());
    
}
