using AssetsNet.API.DTOs.Liqpay;

namespace AssetsNet.API.Interfaces.Liqpay;
public interface IPaymentService
{
    LiqpayResponseDto GeneratePaymentUrl(LiqpayRequestDto request);
}