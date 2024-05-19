using Zad3.Exceptions;
using Zad3.OrderSystem.Models;
using Zad3.OrderSystem.Models.DTOs;

namespace Zad3.OrderSystem
{
    public class WarehouseService
    {
        private readonly WarehouseRepository _warehouseRepository;
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;

        public WarehouseService(WarehouseRepository warehouseRepository, ProductRepository productRepository, OrderRepository orderRepository)
        {
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<int> AddProduct(AddProductDto request)
        {
            var product = await _productRepository.GetProduct(request.IdProduct);
            if (product is null)
                throw new ValidationException($"This product {request.IdProduct} don't exist");

            var warehouse = await _warehouseRepository.GetWarehouse(request.IdWarehouse);
            if (warehouse is null)
                throw new ValidationException($"This warehouse {request.IdWarehouse} don't exist");

            if(request.Amount <= 0)
                throw new ValidationException($"Amount can't be less that 0");

            var order = await _orderRepository.GetOrdersByProduct(request.IdProduct);
            if(order is null)
                throw new ValidationException($"Order for this product dont exist");

            if (order.Amount != request.Amount)
                throw new ValidationException($"Order and request have different amounts");

            if (order.CreatedAt >= request.CreatedAt)
                throw new ValidationException($"Wrong dates");

            var orderRealization = _orderRepository.GetOrderRealizationByOrder(order.IdOrder);
            if (orderRealization is null)
                throw new ValidationException($"This order aren't realized");

            await _orderRepository.UpdateFullfilledAt(order.IdOrder, DateTime.Now);
          
            var realization = new ProductWarehouse()
            {
                IdWarehouse = request.IdWarehouse,
                IdProduct = request.IdProduct,
                IdOrder = order.IdOrder,
                Amount = request.Amount,
                Price = product.Price * request.Amount,
                CreatedAt = DateTime.Now,
            };

            return await _orderRepository.AddOrderRealization(realization);
        }
    }
}