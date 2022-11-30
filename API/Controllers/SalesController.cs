using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PotShop.API.Helpers;
using System;
using System.IO;
using System.Security.Cryptography;

namespace PotShop.API.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicies.RequireManager)]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        [HttpGet]
        public IActionResult test()
        {
            string original = "Here and there and everywhere";
            byte[] encrypted;
            byte[] key;

            using (Aes myAes = Aes.Create())
            {
                key = myAes.Key;
                
                encrypted = AesEncryptStringToBytes(original, myAes.Key, myAes.IV);

                string roundTrip = AesDecryptStringFromBytes(encrypted, myAes.Key, myAes.IV);

                Console.WriteLine("Original: {0}", original);
                Console.WriteLine("Encrypted: {0}", encrypted);
                Console.WriteLine("Round Trip: {0}", roundTrip);
            }

            return Ok(key);
        }


        private static byte[] AesEncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments
            if (plainText == null || plainText.Length <= 0) throw new ArgumentNullException("plaintext");
            if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0) throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create Aes object
            // With specific key and IV

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create encrypter to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create streams used for the encryption

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // This part will write the plain text to out stream
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

            }
            return encrypted;
        }

        private static string AesDecryptStringFromBytes(byte[] cipherBytes, byte[] Key, byte[] IV)
        {
            // Check arguments
            if (cipherBytes == null || cipherBytes.Length <= 0) throw new ArgumentNullException("cipherBytes");
            if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0) throw new ArgumentNullException("IV");

            string plainText = null;

            // Create Aes object
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }

    }
}
