namespace WebMarket.Contracts
{
    public record UserInfoResponse(
        int Id, string Login, string Email, string? Address, decimal? Wallet
        );
}
