using System;
using ReferenceArchitecture.Domain.Orders;

namespace ReferenceArchitecture.Application.Adapters.Orders
{
	internal static class ShippingResultAdapter
	{
		/// <summary>
		/// Used to keep the domain enum free to change.
		/// </summary>
		private enum ShippingResultContract : byte
		{
			// Contract values - DO NOT RENAME
			Unshipped = ShippingResult.Unshipped,
			Shipped = ShippingResult.Shipped,
			Cancelled = ShippingResult.Cancelled,
		}

		public static string GetContractValue(ShippingResult shippingResult)
		{
			if (!Enum.IsDefined(typeof(ShippingResult), shippingResult))
				throw new ArgumentOutOfRangeException(nameof(shippingResult));

			var contractValue = (ShippingResultContract)shippingResult;

			if (!Enum.IsDefined(typeof(ShippingResultContract), contractValue))
				throw new NotImplementedException("Missing adapter value.");

			return contractValue.ToString();
		}
	}
}
