using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Infrastructure.Databases.EntityConfigurations
{
	internal class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			// TODO Enhancement: Most of this can be automated by convention, with a proper library (e.g. builder.DomainEntity() or builder.DomainAggregate())

			builder.Property(e => e.Id)
				.ValueGeneratedNever();

			builder.Property(e => e.CreationDateTime);

			// Single-valued ValueObjects are mapped using HasConversion()
			builder.Property(e => e.Description)
				.IsRequired()
				.HasConversion(e => e.Value, str => new OrderDescription(str))
				.HasMaxLength(OrderDescription.MaxLength);

			// Multi-valued ValueObjects are mapped using OwnsOne()
			builder.OwnsOne(e => e.ShippingStatus, shippingStatus =>
			{
				shippingStatus.Property(s => s.DateTime)
					.HasColumnName("ShippingDateTime");

				shippingStatus.Property(s => s.Result)
					.HasColumnName("ShippingResult")
					.HasConversion<string>()
					.HasMaxLength(50);
			}).Navigation(e => e.ShippingStatus).IsRequired();

			builder.HasKey(e => e.Id);
			builder.HasIndex(e => e.CreationDateTime);
			builder.HasIndex(e => e.ShippingStatus.DateTime);

			// #TODO: Row version
		}
	}
}
