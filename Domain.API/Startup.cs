using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.OData.Edm;
using Domain.API.Profiles;
using SQLDataLayer;
using Microsoft.EntityFrameworkCore;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using Domain.Data;

namespace Domain.API
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
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddApiVersioning();
            services.AddOptions();
            //Update the AddOData Section to here
            services.AddControllers().AddOData(options => options.Select()
                         .OrderBy()
                         .Filter()
                         .SkipToken()
                         .SetMaxTop(null)
                         .Expand()
                         .Count());

            AddSwagger(services);

            //Use an In Memory DB
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("SampleDB"));

            services.AddODataQueryFilter();

            var apiKey = Configuration.GetValue<string>("ApiKey");


            services.AddAutoMapper(typeof(ExampleDataProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.EnvironmentName != "Release")
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseCors(builder => builder
                .AllowAnyOrigin() //In Production make sure to allow only safe origins.
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Use odata route debug, /$odata
            app.UseODataRouteDebug();

            // If you want to use /$openapi, enable the middleware.
            //app.UseODataOpenApi();

            // Add OData /$query middleware
            app.UseODataQueryRequest();
            // Add the OData Batch middleware to support OData $Batch
            app.UseODataBatching();

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings
                {
                    ClientId = Configuration["AzureAd:ClientId"],
                    AppName = "Template",
                };

            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });

        }
        //In case you need to generate an EdmModel
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ExampleData>("ExampleDatas");
            return builder.GetEdmModel();
        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddOpenApiDocument(doc =>
            {
                BaseConfigure(doc, "v1");
                AddSecurity(doc);
            });
            services.AddOpenApiDocument(doc =>
            {
                BaseConfigure(doc, "v2");
                AddSecurity(doc);
            });
        }
        private void AddSecurity(AspNetCoreOpenApiDocumentGeneratorSettings doc)
        {
            
            doc.AddSecurity("apikey", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                Name = "ApiKey",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Description = "ApiKey"
            });
            doc.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("apikey"));
        }
        private void BaseConfigure(AspNetCoreOpenApiDocumentGeneratorSettings doc, string version)
        {

            doc.DocumentName = version;
            doc.ApiGroupNames = new[] { version };

            doc.PostProcess = document =>
            {
                document.Info.Version = version;
                document.Info.Title = "Sample API";
                document.Info.Description = "Starter API for .Net 6";
                document.Info.TermsOfService = "None";
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Tyler Furrer",
                    Email = "Tylerjayfurrer@gmail.com"
                };
            };
        }
    }
}
