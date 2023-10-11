

namespace ChatAppDemo;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ConnectionManager>();
        services.AddSingleton<ChatHandler>();

    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ChatHandler handler) 
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            
        }

        var webSocketOptions = new WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120),
            ReceiveBufferSize = 1024 * 4
        };

        app.UseWebSockets(webSocketOptions);
        app.UseMiddleware<WebSocketMiddleware>(handler);

    }
    

}