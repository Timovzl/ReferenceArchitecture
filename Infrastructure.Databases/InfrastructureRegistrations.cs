using Architect.EntityFramework.AmbientDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReferenceArchitecture.Domain.Databases;

namespace ReferenceArchitecture.Infrastructure.Databases
{
	public static class InfrastructureRegistrations
	{
		public static IServiceCollection AddDatabaseInfrastructure(this IServiceCollection services)
		{
			services.AddPooledDbContextFactory<ReferenceDbContext>(context => context.UseSqlite("Filename=:memory:"));

			services.AddDbContextScope<IReferenceDatabase, ReferenceDbContext>(scope =>
				scope.ExecutionStrategyOptions(ExecutionStrategyOptions.RetryOnOptimisticConcurrencyFailure));

			return services;
		}
	}
}
