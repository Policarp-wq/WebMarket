namespace WebMarket.Contracts.Review
{
    public record ReviewPresentation(int id, string UserEmail, string ReviewContent, short SetRating, DateTime CreatedAt);
}
