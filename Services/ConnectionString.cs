namespace WebMarket.Services
{
    public class ConnectionString
    {
        //exmp "server=%DB_SERVER%;port=5432;database=market;uid=%DB_USER%;password=%DB_PASSWORD%"
        private string _raw;
        public ConnectionString(string? connectionStrigRaw)
        {
            connectionStrigRaw ??= string.Empty;
            _raw = connectionStrigRaw;
        }

        private string GetEnvironmentVariable(string name)
        {
            var res = Environment.GetEnvironmentVariable(name);
            return res == null ? string.Empty : res;
        }

        private string ReplaceEnvVar(string where, string name)
        {
            return where.Replace($"%{name}%", GetEnvironmentVariable(name));
        }

        public string GetReplacedEnvVariables()
        {
            var res = ReplaceEnvVar(_raw,"DB_SERVER");
            res = ReplaceEnvVar(res, "DB_USER");
            res = ReplaceEnvVar(res, "DB_PASSWORD");
            return res;
        }
    }
}
