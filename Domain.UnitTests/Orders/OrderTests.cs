using System;
using Architect.AmbientContexts;
using Architect.Identities;
using ReferenceArchitecture.Domain.Orders;
using ReferenceArchitecture.Testing.Common.DomainBuilders;
using Xunit;

namespace ReferenceArchitecture.Domain.UnitTests.Orders
{
	public class OrderTests
	{
		[Fact]
		public void Construct_Regularly_ShouldReturnExpectedResult()
		{
			using var idScope = new IdGeneratorScope(new CustomIdGenerator(id: 1));
			using var clockScope = new ClockScope(() => DateTime.UnixEpoch);

			var orderBuilder = new OrderBuilder()
				.WithDescription("Description");

			var order = orderBuilder.Build();

			Assert.Equal(1, order.Id);
			Assert.Equal(DateTime.UnixEpoch, order.CreationDateTime);
			Assert.Equal(order.CreationDateTime, order.ShippingStatus.DateTime);
			Assert.Equal(ShippingResult.Unshipped, order.ShippingStatus.Result);
			Assert.Equal("Description", order.Description);
		}
	}
}
