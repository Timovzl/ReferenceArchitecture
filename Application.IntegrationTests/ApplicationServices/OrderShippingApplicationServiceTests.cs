using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReferenceArchitecture.Application.ApplicationServices;
using ReferenceArchitecture.Infrastructure.Databases;
using Xunit;

namespace ReferenceArchitecture.Application.IntegrationTests.ApplicationServices
{
	public class OrderShippingApplicationServiceTests
	{
		private DbConnection Connection { get; }
		private HostBuilder HostBuilder { get; } = new HostBuilder();

		private IHost Host => this._host ??= this.CreateHost();
		private IHost? _host;

		private OrderShippingApplicationService ApplicationService => this.Host.Services.GetRequiredService<OrderShippingApplicationService>();

		private ReferenceDbContext DbContext => this.Host.Services.GetRequiredService<IDbContextFactory<ReferenceDbContext>>().CreateDbContext();

		public OrderShippingApplicationServiceTests()
		{
			this.Connection = new SqliteConnection("Filename=:memory:");
			this.Connection.Open();

			// For some reason Entity Framework uses "first registration wins" instead of "last registration wins", so configure our DbContext for testing first
			this.HostBuilder.ConfigureServices(services => services.AddPooledDbContextFactory<ReferenceDbContext>(
				context => context.UseSqlite(this.Connection)));

			this.HostBuilder.ConfigureServices(services => services.AddReferenceApplication());
		}

		private IHost CreateHost()
		{
			var host = this.HostBuilder.Build();
			host.Services.GetRequiredService<IDbContextFactory<ReferenceDbContext>>().CreateDbContext().Database.EnsureCreated();
			return host;
		}

		[Fact]
		public async Task GetOrderStatus_WithNonexistentOrder_ShouldThrow()
		{
			await Assert.ThrowsAsync<KeyNotFoundException>(() => this.ApplicationService.GetOrderStatus(orderId: 999));
		}

		[Fact]
		public async Task CreateOrder_Regularly_ShouldHaveExpectedResult()
		{
			await this.ApplicationService.CreateOrder("Henk");

			Assert.Single(this.DbContext.Orders);
			Assert.Equal("Henk", this.DbContext.Orders.Single().Description);
		}
	}
}
