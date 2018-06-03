using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypting
{
    public class CrypterRSA
    {
        bool commentMode = true;
        bool immitationMode = false;
        private string key;
        private string keysDirectoryPath = "C:\\Users\\npofa\\source\\repos\\ChatSolution\\ChatProject\\keys/";
        private const string keyFileName = "key_2.xml";

        public CrypterRSA()
        {
            try
            {
                key = File.ReadAllText(keysDirectoryPath + keyFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public string Decrypt(string text)
        {
            if (immitationMode)
                return text;
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
            if (immitationMode)
                return text;
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
    }
}
