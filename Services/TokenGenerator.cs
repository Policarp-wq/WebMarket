using System.Text;

namespace WebMarket.Services
{
    public static class TokenGenerator
    {
        private const string Characters = "qwertyuiopasdfghjklzxcvbnm";
        private static Random Generator = new();
        public static string GetToken(int length)
        {
            StringBuilder token = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                token.Append(Characters[Generator.Next(Characters.Length)]);
            }
            return token.ToString();
        }
    }
}
