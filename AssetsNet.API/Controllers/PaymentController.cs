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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    [HttpGet("liqpay-state/{orderId}")]
    [ProducesResponseType(typeof(PaymentStateResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaymentStateResponseDto>> GetPaymentState([FromRoute] string orderId)
    {
        try
        {
            var response = await _paymentService.GetPaymentState(orderId);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}