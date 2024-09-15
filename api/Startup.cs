using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using StaMemory.Database;
using StaMemory.Repositories;
using StaMemory.Services;

namespace StaMemory;

public class Startup
{
    private const string MyCorsPolicyName = "MyCorsPolicyName";

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        CurrentEnvironment = env;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment CurrentEnvironment { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTIONSTRING_STAMEMORY");

        var client = new MongoClient(connectionString);

        services.AddDbContext<StaMemoryDbContext>(options => options.UseMongoDB(client, "stamemory"));

        services.AddScoped<IDifficultyService, DifficultyService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IRankingService, RankingService>();
        services.AddScoped<ISeasonService, SeasonService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        if (CurrentEnvironment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyCorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4173", "http://localhost:5173")
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .WithHeaders("Content-Type", "PlayerId");
                });
            });
        }

        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        if (env.IsDevelopment())
        {
            app.UseCors(MyCorsPolicyName);
        }

        app.UseAuthorization();

        // if (env.IsDevelopment())
        // {
        //     app.Use(async (context, next) =>
        //     {
        //         await Task.Delay(TimeSpan.FromMilliseconds(500));
        //         await next.Invoke();
        //     });
        // }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}