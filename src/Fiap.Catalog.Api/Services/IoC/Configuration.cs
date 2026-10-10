using Fiap.Catalog.Domain.Repositories;
using Fiap.Catalog.Domain.Services.Interface;
using Fiap.Catalog.Infra.Repositories;
using MessageProducer.Services;

namespace Fiap.Catalog.Api.Services.IoC
{
	/// <summary>
	/// Classe de configuração de IoC
	/// </summary>
	public static class Configuration
	{
		/// <summary>
		/// Registrar as dependencias dentro do motor de IoC
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
		{
      //Interfaces
      services.AddScoped<IRabbitMQService, RabbitMQService>();

      // Repositórios
      services.AddScoped<IJogoRepository, JogoRepository>();
      services.AddScoped<IJogoBibliotecaRepository, JogoBibliotecaRepository>();

      // Domain Services			
      services.AddScoped<Domain.Services.JogoService>();
      services.AddScoped<Domain.Services.BibliotecaService>();

      return services;
		}
	}
}
