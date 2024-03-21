namespace AssetsNet.API.Models.Twitter;

public class TwitterRootObject
{
   public List<TwitterPost> Timeline { get; set; } = new();
}
