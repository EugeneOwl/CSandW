using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypting
{
    public class CrypterRSA
    {
        bool commentMode = true;
        private string key = "<RSAKeyValue><Modulus>lIiP6oG0rWkZ1FR+XUWesTxyd5wqIk6QbXD3wJbsbeui52ob+V8Z2FnZKMkNqcdG8NExJXVfYHqWWJfM/f9uJAJQEtDdKoq8XLpoOvU2diBCzt1ZEQ73Zd2uWbpCp2hLdV0CGnDrCrw73Hpf8YQHfOxArfinz4bf8/NjlOGysP0=</Modulus><Exponent>AQAB</Exponent><P>wUWQ7OJCLzprCHz1pK32OoQprnokxU+8W1lR/hvVvjd3baTpueZrfqffuGlUNJDMHtcFCHRrn12o6lvXpdkvaw==</P><Q>xL3PbHnu8tQHxEKIsCYMXfBSoWMZ8vGWmW1R1Ha2HgaQtxBOi08Vnx44XBM08SGl3/Z/I8flx3Q91IrHxIHDNw==</Q><DP>OGICEIlFQ5/oP7asttklPxb1TfhGJ+XGDmQ1XktihLaLucnlgZ0t+Ooigxr/PMKNOeYCit/j0b8yGwSmGbUNdw==</DP><DQ>I7lDfLL3TGhrsJtULof6HuSQXHWeNJrheaJ8BVQ45WLTzCnN2UXW6zmVikKCjnZJpE0h8OFPO9RDleBNPcVCKQ==</DQ><InverseQ>qjhIBXszf3BW3So4MgQkFlfvUfx7w89TYW/TrwHYBVF0thWLjuhicKhvbyJqlaJT4diEZl5uzFC/G+WlKRyvUA==</InverseQ><D>QEjKYKg6VkMCls7q0dtrzzFmKTZBVg/CmlsmHwgy03AEVTurV8Y9HVHllq1NaImCvn8LUjb2TH9Byp4IbnW6aNp7J1JpU4hmnLPrL5gKhA3m0fvB90roCk8a2bOomIOI3BhdzhU0wJcASTv/rBAHpm8WRKTgcPnWmwh4fJRmOVE=</D></RSAKeyValue>";
        private string key2 = "<RSAKeyValue><Modulus>zYg/nZFhCjiA2muf7nAZny7ksksRmUfVBBtRVkftM4+uO+m7rBZxFeEL8F0HHw84WrJxNQZlwznb9j5ycTKDQ2EpT+hhCzFRicuWuta+59LgBF5qiqhQfZDYwHMOXDcmFGYmvF0lm9nvuKBeDQUXyJWxf5LWQPmbU/jvg1RoRik=</Modulus><Exponent>AQAB</Exponent><P>5ZYAr7VuqDs9b6/OngxLIjHzexBOOLQX4F2TWzmk5LjjElMp+cZkhuAaaCZGJS7bhZNWfo89lW7wI5VQqYpUUw==</P><Q>5S3Kh+88VpkoEnNLNLWLXiUvqpqWEt+qMFpET91PAbkzStInjUJJA7PwvmUtuBkK7/GVqH4/ZUjI50QdrAtsEw==</Q><DP>R581FZuXKtpYPyhsX7fcFI3atFCQ5nWgYTkwCCyCeWwIQqY5GRfAMqdk9YrDJURp7VDakd7jymNyfNdO86UYyw==</DP><DQ>2Sckat1CnQOONr1MG2uR3Oj7W3oSjVulFR/S7qHSRN3JPjIXDAAs0vdjO+T/Bxujg2uz1O2bAEuRIaEnRGemRQ==</DQ><InverseQ>wDpuPmfz2p7zeLvx3ZElxRnBPedHI6PvraEm9km3ngdbzY351INjK5Ev+moKFTdU8snDXQj6vD0Wl8ahD1ECaA==</InverseQ><D>q4SPKgwfQitDBIOaFJ4CE8BLY6qXxiWW03WEnA8sGMJHsYHtdr/HoW2LzAumHLpT2gRdytVK6O+I62bTlDmGJZ6/m0rt+Q1JN5YAY5dzghgxWFpGPSvY6FQVooz1n1zxyAZ141VJEWqyZANsxxjdN6SH7oKdKUq1YB5d2SmKP80=</D></RSAKeyValue>";

        public string Decrypt(string text)
        {
            byte[] decrContent = null;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            if (commentMode)
                Console.WriteLine("Going to decrypt: |" + text + "|");
            decrContent = rsa.Decrypt(Convert.FromBase64String(text), true);

            if (commentMode)
                Console.WriteLine("Decrypted: |" + _toString(decrContent) + "|");
            return _toString(decrContent);
        }

        private string _toString(byte[] decrContent)
        {
            return Encoding.UTF8.GetString(decrContent);
        }

        private byte[] _toByte(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public string Encrypt(string text)
        {
            byte[] encContent = null;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            if (commentMode)
                Console.WriteLine("Going to encrypt: |" + text + "|");
            encContent = rsa.Encrypt(_toByte(text), true);

            if (commentMode)
                Console.WriteLine("Encrypted: |" + Convert.ToBase64String(encContent) + "|");
            return Convert.ToBase64String(encContent);
        }

        private void SetKey(string keyPattern)
        {

        }
    }
}
