using Core.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Order;

public class CreateOrderDto
{
    [Required] public string CartId { get; set; } = string.Empty;
    [Required] public Guid DeliveryMethodId { get; set; }
    [Required] public ShippingAddress ShippingAddress { get; set; } = null!;
    [Required] public PaymentSummary PaymentSummary { get; set; } = null!;
}
