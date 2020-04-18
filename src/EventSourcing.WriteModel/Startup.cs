namespace EventSourcing.WriteModel
{
    using CorrelationId;
    using ExceptionHandler;
    using ExceptionHandler.Middleware;

    using Infrastructure.Configs;
    using Infrastructure.IoC;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;

    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var builder = services.AddMvcCore();
            builder.AddApiExplorer();
            builder.AddFormatterMappings();
            builder.AddDataAnnotations();
            builder.AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            //builder.AddJsonFormatters();

            //builder.AddJsonOptions(settings =>
            //{
            //    settings.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //});

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Event Sourcing PoC",
                    Version = "v1"
                });
            });

            services.Configure<ElasticSearchConfig>(this.Configuration.GetSection("App:elasticSearch"));
            services.Configure<MongoDbConfig>(this.Configuration.GetSection("App:mongodb"));

            services.LoadPersistenceConfigurations(this.Configuration);
            services.LoadApplicationConfigurations();
            services.LoadEventHandlersConfigurations();

            services.AddExceptionHandling(new GlobalExceptionHandler());

            services.AddCorrelationId();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandlingMiddleware();

            app.UseCorrelationId("es-id");

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Sourcing Poc");
            });
        }
    }
}