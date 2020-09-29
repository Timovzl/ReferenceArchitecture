using System;
using System.Linq;
using System.Threading.Tasks;
using Architect.EntityFramework.AmbientDbContexts;
using Microsoft.EntityFrameworkCore;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Infrastructure.Databases.Repos
{
	internal class OrderRepo : IOrderRepo
	{
		/// <summary>
		/// Used to define the base of our loads queries once.
		/// </summary>
		private IQueryable<Order> BaseSelectQuery => this.DbContext.Orders;

		private IDbContextAccessor<ReferenceDbContext> DbContextAccessor { get; }
		private ReferenceDbContext DbContext => this.DbContextAccessor.CurrentDbContext;

		public OrderRepo(IDbContextAccessor<ReferenceDbContext> dbContextAccessor)
		{
			this.DbContextAccessor = dbContextAccessor ?? throw new ArgumentNullException(nameof(dbContextAccessor));
		}

		public Task AddOrder(Order order)
		{
			this.DbContext.Orders.Add(order);
			return Task.CompletedTask;
		}

		public async Task<Order?> GetOrderById(long orderId)
		{
			return await this.BaseSelectQuery
				.SingleOrDefaultAsync(o => o.Id == orderId);
		}
	}
}
