namespace WebMarket.Contracts.Review
{
    public record ReviewAddInfo(int UserId, int ProductId, short SetRating, string ReviewContent);
}
