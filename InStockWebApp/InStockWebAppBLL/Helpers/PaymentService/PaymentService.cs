using InStockWebAppBLL.Features.Interfaces.Domain;
using InStockWebAppDAL.Entities;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;
using Stripe;
using InStockWebAppBLL.Features.Interfaces;

public class PaymentService : IpaymentService
{
    private readonly IConfiguration _configuration;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IConfiguration configuration, ICartRepository cartRepository , IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> CreatePaymentSession(string userId, string Link)
    {
        SetStripeApiKey();
        var cart = await _cartRepository.GetCart(userId);
        if (cart != null)
        {
            var sessionOptions = PrepareSessionOptions(cart,Link);
            var session = await CreateStripeSession(sessionOptions);
            cart.CheckoutSessionId = session.Id;
            await _unitOfWork.Save();
            return session.Url;
        }
        return null;
    }

    private void SetStripeApiKey()
    {
        var apiKey = _configuration["StripeKeys:SecretKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Stripe API key is not configured.");
        }
        StripeConfiguration.ApiKey = apiKey;
    }

    private SessionCreateOptions PrepareSessionOptions(Cart cart,string link)
    {
        decimal shippingPrice = 0;

        var productsPrice = _cartRepository.CalculateCartTotalPrice(cart);
        var lineItems = CreateLineItems(cart);
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = link,
            CancelUrl = "https://example.com/cancel",
        };

        return options;
    }

    private List<SessionLineItemOptions> CreateLineItems(Cart cart)
    {
        var lineItems = new List<SessionLineItemOptions>();

        foreach (var cartItem in cart?.Items) 
        {
            var product = cartItem.Product; 
            var lineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(product.Price * 100), 
                    Currency = "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Images = new List<string> { product.ImgeUrl}, 
                    },
                },
                Quantity = cartItem.Quantity, 
            };

            lineItems.Add(lineItem);
        }

        return lineItems;
    }

    private async Task<Session> CreateStripeSession(SessionCreateOptions options)
    {
        var sessionService = new SessionService();
        var session = await sessionService.CreateAsync(options);
        return session;
    }
  
}
