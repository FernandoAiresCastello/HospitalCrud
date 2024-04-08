using HospitalCrud.Data;
using HospitalCrud.Mappers;
using HospitalCrud.Repositories;
using HospitalCrud.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HospitalCrud
{
	/// <summary>
	/// Handles the various steps required to configure the application
	/// </summary>
	public class ApplicationSetup
	{
		private readonly string ApiName = "HospitalCrud API";
		private readonly string ApiVersion = "v1";
		private readonly string AppSettingsFilename = "appsettings.json";
		private readonly string ConnectionStringKey = "Postgres";

		public WebApplication ConfigureAndBuildApplication()
		{
			var builder = WebApplication.CreateBuilder();
			var appSettings = BuildConfigurationFile(builder);

			EnableApiControllersAndViews(builder);
			RegisterServicesForDependencyInjection(builder);
			RegisterDatabaseContext(builder, appSettings);
			SetupSwaggerGenerator(builder);

			return BuildWebApplication(builder);
		}

		private void EnableApiControllersAndViews(WebApplicationBuilder builder)
		{
			builder.Services.AddControllers();
			builder.Services.AddControllersWithViews();
		}

		private void RegisterServicesForDependencyInjection(WebApplicationBuilder builder)
		{
			builder.Services.AddTransient<IPatientRepository, PatientRepository>();
			builder.Services.AddTransient<IPatientService, PatientService>();
			builder.Services.AddTransient<IPatientMapper, PatientMapper>();
		}

		private void RegisterDatabaseContext(WebApplicationBuilder builder, IConfigurationRoot appSetings)
		{
			builder.Services.AddDbContext<DatabaseContext>(options =>
			{
				string? connectionString = appSetings.GetConnectionString(ConnectionStringKey);

				if (string.IsNullOrEmpty(connectionString))
					throw new KeyNotFoundException($"Connection string '{ConnectionStringKey}' não encontrada no {AppSettingsFilename}");

				options.UseNpgsql(connectionString);
			});

		}

		private IConfigurationRoot BuildConfigurationFile(WebApplicationBuilder builder)
		{
			return new ConfigurationBuilder()
				.SetBasePath(builder.Environment.ContentRootPath)
				.AddJsonFile(AppSettingsFilename, optional: false, reloadOnChange: true)
				.Build();
		}

		private void SetupSwaggerGenerator(WebApplicationBuilder builder)
		{
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
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
