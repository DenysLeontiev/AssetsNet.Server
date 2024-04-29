using System.ComponentModel.DataAnnotations;
using AssetsNet.API.Helpers;

namespace AssetsNet.API.DTOs.Liqpay;
public class LiqpayRequestDto
{
    public TariffPlansEnum TariffPlan { get; set; }
}