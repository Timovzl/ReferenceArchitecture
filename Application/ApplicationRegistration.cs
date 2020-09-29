using Microsoft.Extensions.DependencyInjection;
using ReferenceArchitecture.Infrastructure.Databases;

namespace ReferenceArchitecture.Application
{
	public static class ApplicationRegistration
	{
		public static IServiceCollection AddReferenceApplication(this IServiceCollection services)
		{
			services.AddDatabaseInfrastructure();

			return services;
		}
	}
}
