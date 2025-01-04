namespace WebMarket.Contracts
{
    public record ProductInfo(string Name, string? Description, int Price, string? Image, short? Rating, int CategoryId);
}
