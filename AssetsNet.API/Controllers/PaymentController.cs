using AssetsNet.API.Controllers.Common;
using AssetsNet.API.DTOs.Liqpay;
using AssetsNet.API.Interfaces.Liqpay;
using Microsoft.AspNetCore.Mvc;

namespace AssetsNet.API.Controllers;
public class PaymentController : BaseApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("liqpay-url")]
    [ProducesResponseType(typeof(LiqpayResponseDto), StatusCodes.Status200OK)] 
    public ActionResult<LiqpayResponseDto> GeneratePaymentUrl([FromQuery] LiqpayRequestDto request)
    {
        try
        {
            var response = _paymentService.GeneratePaymentUrl(request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}