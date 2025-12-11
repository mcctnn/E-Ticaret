using API.Dtos.Order;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(IUnitOfWork unitOfWork, ICartService cartService) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
    {
        var email = User.GetEmail();
        var cart = await cartService.GetCartAsync(orderDto.CartId);
        if (cart == null) return BadRequest("Cart not found");
        if (cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");

        var items = new List<OrderItem>();
        foreach (var item in cart.Items)
        {
            var productItem = await unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId, CancellationToken.None);
            if (productItem == null) return BadRequest("Problem with the order");
            var itemOrdered = new ProductItemOrdered { ProductId = item.ProductId, PictureUrl = item.PictureUrl, ProductName = item.ProductName };

            var orderItem = new OrderItem { ItemOrdered = itemOrdered, Price = productItem.Price, Quantity = item.Quantity };
            items.Add(orderItem);
        }

        var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId, CancellationToken.None);
        if (deliveryMethod == null) return BadRequest("No delivery method selected");
        var order = new Order
        {
            BuyerEmail = email,
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            PaymentIntentId = cart.PaymentIntentId,
            PaymentSummary = orderDto.PaymentSummary,
            ShippingAddress = orderDto.ShippingAddress,
            Discount = orderDto.Discount,
            Subtotal = items.Sum(x => x.Price * x.Quantity)
        };
        unitOfWork.Repository<Order>().Add(order);

        if (await unitOfWork.Complete()) return order;

        return BadRequest("Problem creating order");
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
    {
        var spec = new OrderSpecification(User.GetEmail());
        var orders = await unitOfWork.Repository<Order>().ListAsync(spec, CancellationToken.None);
        var ordersToReturn = orders.Select(o => o.ToDto()).ToList();
        return Ok(ordersToReturn);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
    {
        var spec = new OrderSpecification(User.GetEmail(), id);
        var order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec, CancellationToken.None);
        if (order == null) return NotFound();

        return order.ToDto();
    }
}
