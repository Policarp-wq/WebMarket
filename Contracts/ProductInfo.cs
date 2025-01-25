namespace WebMarket.Contracts
{
    public record ProductInfo(string Name, string? Description, double Price, string? Image, double Rating, int CategoryId);
}
