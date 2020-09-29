using System.Data.Common;
using System.Threading.Tasks;
using Architect.EntityFramework.AmbientDbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ReferenceArchitecture.Domain.Orders;
using ReferenceArchitecture.Infrastructure.Databases.Repos;
using Xunit;

namespace ReferenceArchitecture.Infrastructure.Databases.IntegrationTests.Repos
{
	public class OrderRepoTests
	{
		private DbConnection Connection { get; }
		private ReferenceDbContext DbContext { get; }
		private OrderRepo Repo { get; }

		public OrderRepoTests()
		{
			this.Connection = new SqliteConnection("Filename=:memory:");
			this.Connection.Open();

			this.DbContext = new ReferenceDbContext(
				new DbContextOptionsBuilder().UseSqlite(this.Connection).Options);

			this.DbContext.Database.EnsureCreated();

			// Provide the repo access to a fixed DbContext
			var dbContextAccessor = FixedDbContextAccessor.Create(this.DbContext);
			this.Repo = new OrderRepo(dbContextAccessor);
		}

		[Fact]
		public async Task AddOrder_AfterSaving_ShouldAddOrder()
		{
			var order = new Order(new OrderDescription("ExampleOrder"));

			await this.Repo.AddOrder(order);

			this.DbContext.SaveChanges();

			Assert.Single(this.DbContext.Orders);
			Assert.Contains(order, this.DbContext.Orders);
		}
	}
}
