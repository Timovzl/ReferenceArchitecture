using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architect.EntityFramework.AmbientDbContexts;
using ReferenceArchitecture.Domain.Databases;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Application.ApplicationServices
{
	public class OrderShippingApplicationService
	{
		private IDbContextProvider<IReferenceDatabase> DbContextProvider { get; }
		private IOrderRepo OrderRepo { get; }

		public OrderShippingApplicationService(
			IDbContextProvider<IReferenceDatabase> dbContextProvider,
			IOrderRepo orderRepo)
		{
			this.DbContextProvider = dbContextProvider ?? throw new ArgumentNullException(nameof(dbContextProvider));
			this.OrderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
		}

		public async Task<long> CreateOrder(string description)
		{
			var order = new Order(new OrderDescription(description));

			await this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				await this.OrderRepo.AddOrder(order);

				await executionScope.DbContext.SaveChangesAsync();
				executionScope.Complete();
			});

			return order.Id;
		}

		public Task<string> GetOrderStatus(long orderId)
		{
			// #TODO: Fix bug in execution strategy override for MockDbContextProvider

			return this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				var order = await this.OrderRepo.GetOrderById(orderId);

				if (order is null) throw new KeyNotFoundException($"Order {orderId} does not exist.");

				return order.ShippingStatus.ToString(); // #TODO: Add adapter, so that domain model is free to change
			});
		}

		public Task ShipOrder(long orderId)
		{
			return this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				var order = await this.OrderRepo.GetOrderById(orderId);

				if (order is null) throw new KeyNotFoundException($"Order {orderId} does not exist.");

				order.Ship();

				//await this.OrderRepo.Update(order);

				await executionScope.DbContext.SaveChangesAsync();
				executionScope.Complete();
			});
		}
	}
}
