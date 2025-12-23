using API.Dtos.Order;
using API.Extensions;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IUnitOfWork uow, IPaymentService paymentService) : BaseApiController
{
    [HttpGet("orders")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders([FromQuery] OrderSpecParams orderSpecParams)
    {
        var spec = new OrderSpecification(orderSpecParams);
        return await CreatePagedResult(uow.Repository<Order>(), spec, orderSpecParams.PageIndex, orderSpecParams.PageSize, o => o.ToDto(), CancellationToken.None);
    }

    [HttpGet("orders/{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
    {
        var spec = new OrderSpecification(id);

        var order = await uow.Repository<Order>().GetEntityWithSpecAsync(spec, CancellationToken.None);

        if (order == null) return BadRequest("No orders with that id");

        return order.ToDto();
    }

    [HttpPost("orders/refund/{id:guid}")]
    public async Task<ActionResult<OrderDto>> RefundOrder(Guid id)
    {
        var spec = new OrderSpecification(id);
        var order = await uow.Repository<Order>().GetEntityWithSpecAsync(spec, CancellationToken.None);

        if (order == null) return BadRequest("No orders with that id");
        if (order.Status == OrderStatus.Pending)
            return BadRequest("Payment not received for this order");

        var result = await paymentService.RefundPayment(order.PaymentIntentId);
        if (result == "succeeded")
        {
            order.Status = OrderStatus.Refunded;
            await uow.Complete();

            return order.ToDto();
        }

        return BadRequest("Problem refunding order");
    }
}
