namespace WebMarket.Contracts
{
    public record UserInfoPatch(int UserId, string? Login, string? Email, string? Address);
}
