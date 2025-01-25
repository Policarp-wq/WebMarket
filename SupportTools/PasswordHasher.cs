namespace WebMarket.SupportTools
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        public static bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
