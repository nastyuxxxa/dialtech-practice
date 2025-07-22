using Data.Models;

namespace PriceCalculation.Services
{
    public interface IFileWriter
    {
        Task WriteToFileAsync(Product product, string directoryPath);
    }
}
