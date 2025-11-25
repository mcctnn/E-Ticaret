using API.Dtos.Order;
using Core.Entities.OrderAggregate;

namespace API.Extensions;

public static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            BuyerEmail = order.BuyerEmail,
            DeliveryMethod = order.DeliveryMethod.Description,
            ShippingPrice = order.DeliveryMethod.Price,
            OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
            PaymentIntentId = order.PaymentIntentId,
            PaymentSummary = order.PaymentSummary,
            ShippingAddress = order.ShippingAddress,
            Status = order.Status.ToString(),
            Subtotal = order.Subtotal,
            Total = order.GetTotal(),
            OrderDate = order.OrderDate,
        };
    }

    public static OrderItemDto ToDto(this OrderItem orderItem)
    {
        return new OrderItemDto
        {
            ProductId = orderItem.ItemOrdered.ProductId,
            PictureUrl = orderItem.ItemOrdered.PictureUrl,
            ProductName = orderItem.ItemOrdered.ProductName,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity,
        };
    }
}
