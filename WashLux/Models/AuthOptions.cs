using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WashLux.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "WashLux"; // издатель токена
        public const string AUDIENCE = "WashLux"; // потребитель токена
        const string KEY = "1234567890987654321qwertyuiopoiuytrewq";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(KEY));
        }
    }
}
