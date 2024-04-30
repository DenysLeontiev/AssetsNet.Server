using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsNet.API.Entities;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DateSent { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public User? Sender { get; set; }
    public string RecipientId { get; set; } = string.Empty;
    public User? Recipient { get; set; }
}