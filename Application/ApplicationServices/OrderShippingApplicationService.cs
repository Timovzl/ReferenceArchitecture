using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architect.EntityFramework.AmbientDbContexts;
using ReferenceArchitecture.Application.Adapters.Orders;
using ReferenceArchitecture.Domain.Databases;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Application.ApplicationServices
{
	/// <summary>
	/// Provides use cases related to the shipping of <see cref="Order"/>s.
	/// </summary>
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

		public Task ShipOrder(long orderId)
		{
			return this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				var order = await this.OrderRepo.GetOrderById(orderId);

				if (order is null) throw new KeyNotFoundException($"Order {orderId} does not exist.");

				order.Ship();

				// EF with ChangeTracking does not require explicit updates, but the comment is left here for clarification
				//await this.OrderRepo.Update(order);

				await executionScope.DbContext.SaveChangesAsync();
				executionScope.Complete();
			});
		}

		public Task<string> GetOrderShippingResult(long orderId)
		{
			return this.DbContextProvider.ExecuteInDbContextScopeAsync(async executionScope =>
			{
				var order = await this.OrderRepo.GetOrderById(orderId);

				if (order is null) throw new KeyNotFoundException($"Order {orderId} does not exist.");

				// The exposed result becomes part of a contract
				// Avoid leaking parts of the domain model, since being part of a contract would render them hard-to-change
				var result = ShippingResultAdapter.GetContractValue(order.ShippingStatus.Result);
				return result;
			});
		}
	}
}
