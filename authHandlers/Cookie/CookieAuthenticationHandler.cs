using auth.Core.Interfaces;
using auth.Core.Models.Cookie;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace auth.Handlers.Cookie
{
    public class CookieAuthenticationHandler : ICookie
    {
        private readonly string _encryptionKey = Environment.GetEnvironmentVariable("MY_ENCRYPTION_KEY");

        public CookieAuthenticationHandler()
        {

        }

        public async Task<UserDataCookie> SetCookieAuthenticationHandler()
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,"admin"),
                new Claim(ClaimTypes.Email, "admin@mywebsite.com")
            };

            // Ad un'identità è associato 1 o più Claim, questo perchè l'utente può fare l'accesso con diverse identità a cui saranno associate
            // differenti informazioni 

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var serializedIdentity = JsonConvert.SerializeObject(identity);
            var encyptedValue = Encrypt(serializedIdentity, _encryptionKey);


            //Viene creato un ClaimsPrincipal che sarà colui che possiederà il context security
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            // creiamo il cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            return new UserDataCookie { EncryptedValue = encyptedValue, CookieOptions = cookieOptions };
        }


        private string Encrypt(string value, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // L'IV dovrebbe essere generato casualmente, ma per questo esempio lo impostiamo su un array di byte vuoto

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        // Metodo per decifrare una stringa
        private string Decrypt(string encryptedValue, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // L'IV dovrebbe essere generato casualmente, ma per questo esempio lo impostiamo su un array di byte vuoto

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedValue)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}

