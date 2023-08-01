namespace blogpessoal.Security
{
    public class Settings
    {
        private static string secret = "CBA26228506E6FDDF8DE6C108B4F46C5C721362B84388C1CA8A9070E25F120BE";

        public static string Secret { get => secret; set => secret = value; }

    }
}

