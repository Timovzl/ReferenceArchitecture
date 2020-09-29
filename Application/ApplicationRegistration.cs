using Microsoft.Extensions.DependencyInjection;
using ReferenceArchitecture.Application.ApplicationServices;
using ReferenceArchitecture.Infrastructure.Databases;

namespace ReferenceArchitecture.Application
{
	public static class ApplicationRegistration
	{
		public static IServiceCollection AddReferenceApplication(this IServiceCollection services)
		{
			services.AddDatabaseInfrastructure();

			// #TODO: Assembly scanning
			services.AddSingleton<OrderShippingApplicationService>();

			return services;
		}
	}
}
