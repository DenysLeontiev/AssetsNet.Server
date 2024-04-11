using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsNet.API.Entities;

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; }
    public string SenderId { get; set; }
    public User Sender { get; set; }
    public string RecipientId { get; set; }
    public User Recipient { get; set; }
}