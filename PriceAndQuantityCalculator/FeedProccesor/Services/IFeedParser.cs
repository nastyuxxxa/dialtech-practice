using Data.Models;

namespace FeedProcessor.Services
{
    public interface IFeedParser
    {
        List<Product> Parse(string feedContent);
    }
}
