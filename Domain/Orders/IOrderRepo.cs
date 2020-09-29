using System.Threading.Tasks;

namespace ReferenceArchitecture.Domain.Orders
{
	public interface IOrderRepo
	{
		Task AddOrder(Order order);

		Task<Order?> GetOrderById(long orderId);
	}
}
