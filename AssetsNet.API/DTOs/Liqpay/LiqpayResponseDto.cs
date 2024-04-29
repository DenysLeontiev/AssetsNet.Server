namespace AssetsNet.API.DTOs.Liqpay;
public class LiqpayResponseDto
{
    public string OrderId { get; set; } = string.Empty;

    public string PaymentUrl { get; set; } = string.Empty;
}