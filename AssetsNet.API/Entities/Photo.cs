using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsNet.API.Entities;

public class Photo
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; } 
}