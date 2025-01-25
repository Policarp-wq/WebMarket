namespace WebMarket.Contracts
{
    public record ShoppingCartElementPresentation(int ElementId, int ProductId, string Name, int Amount, double UnitPrice, string? Image);
}
