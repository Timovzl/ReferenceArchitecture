using System;
using Architect.AmbientContexts;
using Architect.Identities;
using ReferenceArchitecture.Tools.DomainModeling;

namespace ReferenceArchitecture.Domain.Orders
{
	public class Order : Entity<Order, long>
	{
		public override Identity<Order, long> Id { get; }

		public DateTime CreationDateTime { get; }

		public OrderDescription Description { get; private set; }

		public ShippingStatus ShippingStatus { get; private set; }

		public Order(OrderDescription description)
		{
			// Note how the domain constraints are enforced on construction
			// More specific constraints (such as the contents of OrderDescription) have been enforced on construction of the respective types

			this.Id = IdGenerator.Current.CreateId();

			this.CreationDateTime = Clock.Now;

			this.Description = description ?? throw new ArgumentNullException(nameof(description));

			this.ShippingStatus = new ShippingStatus(this.CreationDateTime, ShippingResult.Unshipped);
		}

		public void ChangeDescription(OrderDescription description)
		{
			this.Description = description ?? throw new ArgumentNullException(nameof(description));
		}

		public void Ship()
		{
			if (this.ShippingStatus.Result != ShippingResult.Unshipped)
				throw new InvalidOperationException($"Unable to ship an {nameof(Order)} with status {this.ShippingStatus}.");

			this.ShippingStatus = new ShippingStatus(Clock.Now, ShippingResult.Shipped);
		}

		public void CancelShipment()
		{
			if (this.ShippingStatus.Result != ShippingResult.Unshipped)
				throw new InvalidOperationException($"Unable to cancel shipment of an {nameof(Order)} with status {this.ShippingStatus}.");

			this.ShippingStatus = new ShippingStatus(Clock.Now, ShippingResult.Cancelled);
		}
	}
}
