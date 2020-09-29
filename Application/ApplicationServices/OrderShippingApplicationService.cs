using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architect.EntityFramework.AmbientDbContexts;
using ReferenceArchitecture.Domain.Databases;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Application.ApplicationServices
{
	internal class OrderShippingApplicationService
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

				// #TODO: Make SaveChanges[Async] accessible from execution scope, even without knowledge of the exact DbContext type
				//await executionScope.DbContext.SaveChangesAsync();
				executionScope.Complete();
			});

			return order.Id;
		}

		public async Task<string> GetOrderStatus(long orderId)
		{
			// #TODO: Reduce overload count by putting TState into executionScope
			// #TODO: Add Task<T> return type overloads

			string result = null!;

			await this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				var order = await this.OrderRepo.GetOrderById(orderId);

				if (order is null) throw new KeyNotFoundException($"Order {orderId} does not exist.");

				result = order.ShippingStatus.ToString(); // #TODO: Add adapter, so that domain model is free to change
			});

			return result;
		}

		public Task ShipOrder(long orderId)
		{
			return this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				var order = await this.OrderRepo.GetOrderById(orderId);

				if (order is null) throw new KeyNotFoundException($"Order {orderId} does not exist.");

				order.Ship();

				//await this.OrderRepo.Update(order);

				// #TODO: Make SaveChanges[Async] accessible from execution scope, even without knowledge of the exact DbContext type
				//await executionScope.DbContext.SaveChangesAsync();
				executionScope.Complete();
			});
		}
	}
}
