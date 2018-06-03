using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProject
{
    public class KeyWordController
    {
        private List<string> keywords = new List<string>();

        private string setKeyWord = "key";

        public KeyWordController()
        {
            keywords.Add(setKeyWord);
        }

        protected internal bool IsKeyword(string message)
        {
            return keywords.Contains(message);
        }

        public bool IsKey(string message)
        {
            if (message.Trim().StartsWith("key "))
                return true;
            return false;
        }

        public string GetKey(string message)
        {
            return message.Substring(setKeyWord.Length + 1);
        }
    }
}
