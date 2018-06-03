using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hashing;

namespace UserProcessing
{
    public class UserProcessor
    {
        private List<User> users = new List<User>();
        private const string UserPasswordFilePath = "C:\\Users\\npofa\\source\\repos\\ChatSolution\\UserProcessing\\users.json";
        private PasswordHasher passwordHasher = new PasswordHasher();

        public UserProcessor()
        {
            DeserializeAllJSON();
        }

        public bool IsUserValid(string login, string password)
        {
            DeserializeAllJSON();
            bool doesUserExist = false;
            foreach (User user in users)
            {
                if (user.login == login)
                    if (passwordHasher.ValidatePassword(password, user.password))
                        doesUserExist = true;
            }
            return doesUserExist;
        }

        public bool IsUsernameFree(string login)
        {
            DeserializeAllJSON();
            foreach (User user in users)
            {
                if (user.login == login)
                    return false;
            }
            return true;
        }

        public void RegistrateUser(string login, string password)
        {
            User user = new User(login, passwordHasher.HashPassword(password));
            users.Add(user);
            ClearFile();
            SerializeAllJSON();
        }

        private void SerializeAllJSON(string path = UserPasswordFilePath)
        {
            var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(users, jset);
            File.WriteAllText(path, json);
        }

        private void DeserializeAllJSON(string path = UserPasswordFilePath)
        {
            string json = File.ReadAllText(path);
            var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            users = (List<User>)JsonConvert.DeserializeObject(json, jset);
        }

        private void ClearFile(string path = UserPasswordFilePath)
        {
            File.WriteAllText(path, string.Empty);
        }
    }
}
