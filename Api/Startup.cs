using Microsoft.OpenApi.Models;
using AlgoBacktesterBackend.Api.Config;

namespace AlgoBacktesterBackend.Api;
public class Startup
{
    private ApplicationSettings _settings;
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _settings = new ApplicationSettings();
        Configuration.Bind(_settings);
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
        services.AddServices(_settings);
        services.AddMocks(_settings);
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AlgoBacktesterBackend", Version = "v1" });
            });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlgoBacktesterBackend v1"));
        }

        if (_settings.UseMocks) {
            Console.WriteLine("Starting API using mocks");
        }
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
