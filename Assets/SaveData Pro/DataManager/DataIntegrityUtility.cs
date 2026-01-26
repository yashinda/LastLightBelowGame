using UnityEngine;
using System.Security.Cryptography;
using System.Text;

namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Data integrity verification utility using Salted Hash
    /// Detects tampering of stored data
    /// </summary>
    public static class DataIntegrityUtility
    {
        /// <summary>
        /// Calculate hash for data with salt from configuration
        /// </summary>
        /// <param name="data">Data to hash</param>
        /// <returns>SHA256 hash as hex string</returns>
        public static string ComputeHash(byte[] data)
        {
            try
            {
                string secretSalt = SaveDataProConfig.Instance.secretSalt;
                // Combine data with salt
                byte[] saltedData = CombineArrays(data, Encoding.UTF8.GetBytes(secretSalt));

                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(saltedData);
                    return System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Hash computation failed: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Verify data integrity
        /// </summary>
        /// <param name="data">Data to verify</param>
        /// <param name="expectedHash">Expected hash</param>
        /// <returns>True if data is intact</returns>
        public static bool VerifyIntegrity(byte[] data, string expectedHash)
        {
            string computedHash = ComputeHash(data);
            return string.Equals(computedHash, expectedHash, System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Combine two byte arrays
        /// </summary>
        private static byte[] CombineArrays(byte[] first, byte[] second)
        {
            byte[] result = new byte[first.Length + second.Length];
            System.Array.Copy(first, 0, result, 0, first.Length);
            System.Array.Copy(second, 0, result, first.Length, second.Length);
            return result;
        }
    }
}