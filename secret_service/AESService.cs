using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cit.secret_service
{
    public class AESService
    {
        private static int KEY_SIZE = 256;
        private static int BLOCK_SIZE = 128;
        private static int LOCAL_SALT_LENGTH = 16;

        public static string EncryptString(string text, string password, string salt)
        {
            if(salt == null)
            {
                salt = "defaultCITSaltToBeUsed";
            }
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            var pwdBytes = Encoding.UTF8.GetBytes(password);
            var pwdHash = SHA256.Create().ComputeHash(pwdBytes);
            var textBytes = Encoding.UTF8.GetBytes(text);
            var textSalt = GetRandomBytes();
            var encrypted = new byte[textSalt.Length + textBytes.Length];

            for (int i = 0; i < textSalt.Length; i++)
                encrypted[i] = textSalt[i];
            for (int i = 0; i < textBytes.Length; i++)
                encrypted[i + textSalt.Length] = textBytes[i];

            encrypted = Encrypt(encrypted, pwdHash, saltBytes);
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptString(string text, string password, string salt)
        {
            if(salt == null)
            {
                salt = "defaultCITSaltToBeUsed";
            }
            var saltBytes = Encoding.UTF8.GetBytes(salt);            
            var pwdBytes = Encoding.UTF8.GetBytes(password);
            var pwdHash = SHA256.Create().ComputeHash(pwdBytes);
            var textBytes = Convert.FromBase64String(text);
            var decrypted = Decrypt(textBytes, pwdHash, saltBytes);

            var result = new byte[decrypted.Length - LOCAL_SALT_LENGTH];
            for (int i = 0; i < result.Length; i++)
                result[i] = decrypted[i + LOCAL_SALT_LENGTH];

            return Encoding.UTF8.GetString(result);
        }

        private static byte[] GetRandomBytes()
        {
            var random = new byte[LOCAL_SALT_LENGTH];
            using(var keyGenerator = RandomNumberGenerator.Create())  
            {  
                keyGenerator.GetBytes(random);  
            }
            return random;
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            using (var AES = Aes.Create())
            using (var resultStream = new MemoryStream())
            {
                AES.KeySize = KEY_SIZE;
                AES.BlockSize = BLOCK_SIZE;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Mode = CipherMode.CBC;

                using (var stream = new CryptoStream(resultStream, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    stream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                }
                encryptedBytes = resultStream.ToArray();
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] decryptedBytes = null;

            using (var AES = Aes.Create())
            using (var resultStream = new MemoryStream())
            {
                AES.KeySize = KEY_SIZE;
                AES.BlockSize = BLOCK_SIZE;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Mode = CipherMode.CBC;

                using (var stream = new CryptoStream(resultStream, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    stream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                }
                decryptedBytes = resultStream.ToArray();
            }
            return decryptedBytes;
        }
    }
}