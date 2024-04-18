using System.ComponentModel.DataAnnotations;
using AssetsNet.API.Helpers;

namespace AssetsNet.API.DTOs;
public class UpdateUserRequestsLimitDto
{
    [Required]
    public TariffPlansEnum TariffPlan { get; set; }
    
    [Required]
    public int PaymentState { get; set; }
}