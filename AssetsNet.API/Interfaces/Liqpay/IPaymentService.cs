using AssetsNet.API.DTOs.Liqpay;

namespace AssetsNet.API.Interfaces.Liqpay;
public interface IPaymentService
{
    LiqpayResponseDto GeneratePaymentUrl(LiqpayRequestDto request);
    Task<PaymentStateResponseDto> GetPaymentState(string orderId);
}