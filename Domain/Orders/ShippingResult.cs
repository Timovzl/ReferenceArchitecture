namespace ReferenceArchitecture.Domain.Orders
{
	public enum ShippingResult : byte
	{
		Unshipped = 0,
		Shipped = 1,
		Cancelled = 9,
	}
}
