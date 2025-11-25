using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;


namespace Infrastructure.Services;

public class PaymentService(IConfiguration cfg,
    IUnitOfWork unitOfWork,
    ICartService cartService) : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntentAsync(string cartId)
    {
        StripeConfiguration.ApiKey = cfg["StripeSettings:SecretKey"];

        var cart = await cartService.GetCartAsync(cartId);
        if (cart == null) return null;

        var shippingPrice = 0m;
        if (cart.DeliveryMethodId.HasValue)
        {
            var dm = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((Guid)cart.DeliveryMethodId, CancellationToken.None);
            if (dm == null) return null;
            shippingPrice = dm.Price;
        }
        foreach (var item in cart.Items)
        {
            var product = await unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId, CancellationToken.None);
            if (product == null) return null;
            if (item.Price != product.Price)
            {
                item.Price = product.Price;
            }
        }

        var service = new PaymentIntentService();
        PaymentIntent? intent = null;
        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)(shippingPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = ["card"],
            };
            intent = await service.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)(shippingPrice * 100),
            };
            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }
        await cartService.SetCartAsync(cart);
        return cart;
    }
}
