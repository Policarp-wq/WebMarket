namespace WebMarket.Contracts
{
    public record UserRegistrationInfo(int UserId, string Login, string Password, string Email, string? Address);
}
