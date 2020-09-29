using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Infrastructure.Databases
{
	internal sealed class ReferenceDbContext : DbContext
	{
		public DbSet<Order> Orders { get; private set; } = null!;

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
