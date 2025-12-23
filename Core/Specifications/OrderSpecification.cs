using Core.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(string email) : base(x => x.BuyerEmail == email)
    {
        AddInclude(x => x.OrderItems);
        AddInclude(x => x.DeliveryMethod);
        AddOrderByDescending(x => x.OrderDate);
    }
    public OrderSpecification(string email, Guid id) : base(x => x.BuyerEmail == email && x.Id == id)
    {
        AddInclude("OrderItems");
        AddInclude("DeliveryMethod");
    }

    public OrderSpecification(string paymentIntentId, bool isPaymentIntent) : base(x => x.PaymentIntentId == paymentIntentId)
    {
        AddInclude("OrderItems");
        AddInclude("DeliveryMethod");
    }

    public OrderSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.OrderItems);
        AddInclude(x => x.DeliveryMethod);
    }

    public OrderSpecification(OrderSpecParams orderSpecParams) : base(x =>
        (string.IsNullOrEmpty(orderSpecParams.Status) || x.Status == ParseOrderStatus(orderSpecParams.Status))
    )
    {
        AddInclude(x => x.OrderItems);
        AddInclude(x => x.DeliveryMethod);
        AddOrderByDescending(x => x.OrderDate);
        ApplyPaging(orderSpecParams.PageSize * (orderSpecParams.PageIndex - 1), orderSpecParams.PageSize);
    }

    private static OrderStatus? ParseOrderStatus(string status)
    {
        return status.ToLower() switch
        {
            "Pending" => OrderStatus.Pending,
            "PaymentReceived" => OrderStatus.PaymentReceived,
            "PaymentFailed" => OrderStatus.PaymentFailed,
            "PaymentMismatch" => OrderStatus.PaymentMismatch,
            _ => null
        };
    }

    private static OrderStatus? ParseStatus(string status) // kullanılabilir
    {
        if (Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
        {
            return parsedStatus;
        }
        return null;
    }
}
