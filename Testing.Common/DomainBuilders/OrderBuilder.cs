using System;
using Architect.Identities;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Testing.Common.DomainBuilders
{
	public class OrderBuilder
	{
		// #TODO: Nullable enable everywhere
		private IIdGenerator? IdGenerator { get; set; } = null;

		private OrderDescription Description { get; set; } = new OrderDescription("DummyDescription");

		public OrderBuilder WithId(long value) => this.With(self => self.IdGenerator = new CustomIdGenerator(value < 0 ? throw new Exception("Invalid ID.") : (ulong)value));
		public OrderBuilder WithDescription(string value) => this.With(self => self.Description = new OrderDescription(value));

		private OrderBuilder With(Action<OrderBuilder> assignment)
		{
			assignment(this);
			return this;
		}

		public Order Build()
		{
			using (this.IdGenerator is null ? null : new IdGeneratorScope(this.IdGenerator))
			{
				var result = new Order(this.Description);
				return result;
			}
		}
	}
}
