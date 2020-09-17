using System;
using ReferenceArchitecture.Tools.DomainModeling;

namespace ReferenceArchitecture.Domain.Orders
{
	public class ShippingStatus : ValueObject
	{
		public override string ToString() => $"{{{this.GetType().Name} Result={this.Result} DateTime={this.DateTime}}}";

		public DateTime DateTime { get; }
		public ShippingResult Result { get; }

		public ShippingStatus(DateTime dateTime, ShippingResult result)
		{
			if (!Enum.IsDefined(typeof(ShippingResult), result))
				throw new ArgumentException($"Unexpected {nameof(ShippingResult)} value: {result}.");

			this.DateTime = dateTime;
			this.Result = result;
		}
	}
}
