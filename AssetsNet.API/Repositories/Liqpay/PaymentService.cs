using AssetsNet.API.DTOs.Liqpay;
using AssetsNet.API.Helpers;
using AssetsNet.API.Interfaces.Liqpay;
using LiqPay.SDK;
using LiqPay.SDK.Dto;

namespace AssetsNet.API.Repositories.Liqpay;
public class PaymentService : IPaymentService
{
    private readonly IConfiguration _config;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IConfiguration config,
        ILogger<PaymentService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public LiqpayResponseDto GeneratePaymentUrl(LiqpayRequestDto request)
    {
        string publicKey = _config["LiqPayApi:PublicKey"];
        string privateKey = _config["LiqPayApi:PrivateKey"];
        string successUrl = _config["LiqPayApi:SuccessUrl"];

        var orderId = Guid.NewGuid().ToString() + "," + DateTime.UtcNow;
        orderId = orderId.Replace("/", "|"); //if we have "/", it breaks url

        var amount = request.TariffPlan switch
        {
            TariffPlansEnum.Basic => TariffPlanConsts.BasicPrice,
            TariffPlansEnum.Premium => TariffPlanConsts.PremiumPrice,
            _ => throw new Exception("Tariff plan is not found")
        };

        LiqPayClient client = new LiqPayClient(publicKey, privateKey);
        
        var paymentParams = new LiqPayRequest
        {
            Version = 3,
            Action = LiqPay.SDK.Dto.Enums.LiqPayRequestAction.Pay,
            Amount = (double)amount,
            Currency = "UAH",
            Description = $"Оплата замовлення #{orderId}",
            OrderId = orderId,
            ResultUrl = successUrl,
        };

        var dataAndSignature = client.PrepareRequestData(paymentParams);

        var url = $"{_config["LiqPayApi:CheckOutUrl"]}?data={dataAndSignature["data"]}&signature={dataAndSignature["signature"]}";

        _logger.LogInformation("Payment url: {url} successfully generated", url);

        return new LiqpayResponseDto
        {
            OrderId = orderId,
            PaymentUrl = url
        };
    }


    
}