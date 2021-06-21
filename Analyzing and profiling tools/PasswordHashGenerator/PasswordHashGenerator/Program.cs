using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordHashGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var password = "myRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorrismyRealyRealyRealyUltraMegaSuperGigaStrongPasswordNotPossibleToGuessEvenByChuckNorris";

            var salt = "JustSaltJustSaltJustSaltJustSalt";

            var saltBuffer = Encoding.UTF8.GetBytes(salt);

            var hash = GeneratePasswordHashUsingSalt(password, saltBuffer);
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 2000000;

            var pbkdf2 = new PasswordDeriveBytes(passwordText, salt, HashAlgorithmName.MD5.ToString(), iterate);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);

            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;

        }
    }
}
