using Architect.EntityFramework.AmbientDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReferenceArchitecture.Domain.Databases;
using ReferenceArchitecture.Domain.Orders;
using ReferenceArchitecture.Infrastructure.Databases.Repos;

namespace ReferenceArchitecture.Infrastructure.Databases
{
	public static class InfrastructureRegistrations
	{
		public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services)
		{
			services.AddPooledDbContextFactory<ReferenceDbContext>(context => context.UseSqlite("Filename=:memory:"));

			services.AddDbContextScope<IReferenceDatabase, ReferenceDbContext>(scope =>
				scope.ExecutionStrategyOptions(ExecutionStrategyOptions.RetryOnOptimisticConcurrencyFailure));

			// #TODO: Assembly scanning
			services.AddSingleton<IOrderRepo, OrderRepo>();

			return services;
		}
	}
}
