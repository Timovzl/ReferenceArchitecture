using System.Collections.Generic;
using System.Threading.Tasks;
using Architect.EntityFramework.DbContextManagement;
using Moq;
using ReferenceArchitecture.Application.ApplicationServices;
using ReferenceArchitecture.Domain.Databases;
using ReferenceArchitecture.Domain.Orders;
using ReferenceArchitecture.Infrastructure.Databases;
using Xunit;

namespace ReferenceArchitecture.Application.UnitTests.ApplicationServices
{
	public class OrderShippingApplicationServiceTests
	{
		private IDbContextProvider<IReferenceDatabase> DbContextProvider { get; }
		private Mock<IOrderRepo> MockOrderRepo { get; }

		private OrderShippingApplicationService ApplicationService { get; }

		public OrderShippingApplicationServiceTests()
		{
			this.DbContextProvider = new MockDbContextProvider<IReferenceDatabase, ReferenceDbContext>();
			this.MockOrderRepo = new Mock<IOrderRepo>();

			this.ApplicationService = new OrderShippingApplicationService(this.DbContextProvider, this.MockOrderRepo.Object);
		}

		[Fact]
		public async Task ShipOrder_WithNonexistentOrder_ShouldThrow()
		{
			const long orderId = 999;

			this.MockOrderRepo.Setup(repo => repo.GetOrderById(orderId))
				.Returns(Task.FromResult((Order?)null));

			await Assert.ThrowsAsync<KeyNotFoundException>(() => this.ApplicationService.ShipOrder(orderId));
		}
	}
}
