using HospitalCrud.Data;
using HospitalCrud.JSONConverters;
using HospitalCrud.Model;
using HospitalCrud.Repositories;
using HospitalCrud.Services;
using HospitalCrud.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;

namespace HospitalCrud
{
	/// <summary>
	/// Handles the various steps required to configure the application
	/// </summary>
	public class ApplicationSetup
	{
		private readonly string ApiName = Constants.ApplicationTitle;
		private readonly string ApiVersion = "v1";
		private readonly string AppSettingsFilename = "appsettings.json";
		private readonly string ConnectionStringKey = "Postgres";

		public WebApplication ConfigureAndBuildApplication()
		{
			var builder = WebApplication.CreateBuilder();
			var appSettings = LoadAppSettings(builder.Environment);
            var services = builder.Services;

            EnableApiControllersAndViews(services);
			ConfigureApiBehaviorOptions(services);
			RegisterServicesForDependencyInjection(services);
			RegisterDatabaseContext(services, appSettings);
			SetupSwaggerGenerator(services);

			return BuildWebApplication(builder);
		}

		private void EnableApiControllersAndViews(IServiceCollection services)
		{
            services.AddControllersWithViews();
			services.AddControllers()
					.AddJsonOptions(ConfigureJsonOptions);
        }

		private void ConfigureJsonOptions(JsonOptions options)
		{
            options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        }

        private void ConfigureApiBehaviorOptions(IServiceCollection services)
		{
            services.Configure<ApiBehaviorOptions>(options =>
            {
				// This configures the API to include a list of errors found in the model
				// whenever it fails to serialize the JSON coming in the request
				options.InvalidModelStateResponseFactory = context =>
				{
                    var errors =
					(
                        from state in context.ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage
                    );

                    return new ApiResponseObject(StatusCodes.Status400BadRequest, 
						ValidationMessages.InvalidJson, errors);
                };
            });
        }

		private void RegisterServicesForDependencyInjection(IServiceCollection services)
		{
			services.AddSingleton<IPatientRepository, PatientRepository>();
			services.AddSingleton<IPatientService, PatientService>();
			services.AddSingleton<IExamplesProvider<Patient>, PatientExample>();
		}

		private void RegisterDatabaseContext(IServiceCollection services, IConfigurationRoot appSetings)
		{
			services.AddDbContext<DatabaseContext>(options =>
			{
				string? connectionString = appSetings.GetConnectionString(ConnectionStringKey);

				if (string.IsNullOrEmpty(connectionString))
					throw new KeyNotFoundException($"Connection string '{ConnectionStringKey}' não encontrada no {AppSettingsFilename}");

				options.UseNpgsql(connectionString);

			}, ServiceLifetime.Singleton);

		}

		private IConfigurationRoot LoadAppSettings(IWebHostEnvironment environment)
		{
			return new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile(AppSettingsFilename, optional: false, reloadOnChange: true)
				.Build();
		}

		private void SetupSwaggerGenerator(IServiceCollection services)
		{
			services.AddSwaggerGen(setup =>
			{
				setup.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

				setup.IncludeXmlComments(xmlPath);
				setup.ExampleFilters();
				setup.EnableAnnotations();
			});

			services.AddSwaggerExamplesFromAssemblyOf<PatientExample>();
		}

		private WebApplication BuildWebApplication(WebApplicationBuilder builder)
		{
			var app = builder.Build();

			app.UseDeveloperExceptionPage();
			app.UseHsts();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();

			app.UseSwagger();
			app.UseSwaggerUI(ui => 
				ui.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", $"{ApiName} {ApiVersion}"));

			app.MapControllers();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Index}/{action=Index}/{id?}");

			return app;
		}
	}
}
