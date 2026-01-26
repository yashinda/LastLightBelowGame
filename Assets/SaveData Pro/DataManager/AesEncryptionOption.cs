using UnityEngine;
using System.IO;
using System.Security.Cryptography;

namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Secure AES-256 encryption implementation
    /// Uses random IV for each encryption to enhance security
    /// </summary>
    public class AesEncryptionOption : IEncryptionOption
    {
        private readonly byte[] key;
        private readonly int aesKeySize;
        private readonly int aesIvSize;

        public AesEncryptionOption(string keyString)
        {
            var config = SaveDataProConfig.Instance;
            this.aesKeySize = config.aesKeySize;
            this.aesIvSize = config.aesIvSize;
            
            // Create key with configured size from input string
            this.key = GenerateKey(keyString, aesKeySize);
        }

        /// <summary>
        /// Generate AES key with specified size
        /// </summary>
        private byte[] GenerateKey(string keyString, int keySize)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(keyString));
                
                // Truncate or repeat hash to achieve desired size
                byte[] key = new byte[keySize];
                for (int i = 0; i < keySize; i++)
                {
                    key[i] = hash[i % hash.Length];
                }
                return key;
            }
        }

        public byte[] Encrypt(byte[] dataToEncrypt)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.GenerateIV(); // Generate new random IV each time
                    byte[] iv = aes.IV;

                    using (var memoryStream = new MemoryStream())
                    {
                        // Write IV to the beginning of stream
                        memoryStream.Write(iv, 0, iv.Length);
                        
                        using (var cryptoStream = new CryptoStream(
                            memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                            cryptoStream.FlushFinalBlock();
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"AES Encryption failed: {e.Message}");
                throw;
            }
        }

        public byte[] Decrypt(byte[] dataToDecrypt)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    
                    // Read IV from beginning of data
                    byte[] iv = new byte[aesIvSize];
                    System.Array.Copy(dataToDecrypt, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(
                            memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            // Decrypt remaining data (after IV)
                            cryptoStream.Write(dataToDecrypt, iv.Length, dataToDecrypt.Length - iv.Length);
                            cryptoStream.FlushFinalBlock();
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"AES Decryption failed: {e.Message}");
                throw;
            }
        }
    }
}
