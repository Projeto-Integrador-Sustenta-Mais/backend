namespace SustentaMais.Security
{
    public class Settings
    {
        private static string secret = "b2be8e3454a52c6c1b5e047c6e20cf20083e38e8aadfb7a115165bf5de955930";
        public static string Secret { get => secret; set => secret = value; }

    }
}
