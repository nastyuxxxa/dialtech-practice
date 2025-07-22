namespace PriceCalculation.Services
{
    public class PriceCalculationOptions
    {
        public string ResultDirectoryPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "Results");
    }
}
