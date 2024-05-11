using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsNet.API.Entities;

public class Request 
{
    public Request()
    {
        RequestedAt = DateTime.Now;
    }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string SenderId { get; set; }
    public User Sender { get; set; }
    public string RequestToAI { get; set; }
    public string ResponseFromAI { get; set; }
    public DateTime RequestedAt { get; set; }
}