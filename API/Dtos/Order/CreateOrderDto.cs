using Core.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Order;

public class CreateOrderDto
{
    [Required] public string CartId { get; set; } = string.Empty;
    [Required] public Guid DeliveryMethodId { get; set; }
    [Required] public ShippingAddress ShippingAddress { get; set; } = null!;
    public decimal Discount { get; set; }
    [Required] public PaymentSummary PaymentSummary { get; set; } = null!;
}
