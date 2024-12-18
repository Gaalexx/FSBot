using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class User
    {
        public String Name { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public long ChatId { get; set; }
        public String ConfirmationСode { get; set; }
        public String Team { get; set; }
        public Boolean WantToGetNews { get; set; }

        static public async Task<string> HeshPassword(string pass)
        {
            Console.WriteLine("Вызвана функция хэширования пароля.");
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pass);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                
                return Convert.ToHexString(hashBytes); 
            }
        }
    }
}
