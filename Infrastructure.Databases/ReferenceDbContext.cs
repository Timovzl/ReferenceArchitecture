using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ReferenceArchitecture.Infrastructure.Databases
{
	internal sealed class ReferenceDbContext : DbContext
	{
		public ReferenceDbContext([NotNull] DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
		}
	}
}
