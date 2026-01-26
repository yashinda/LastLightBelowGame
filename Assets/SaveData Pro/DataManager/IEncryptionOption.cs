namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Interface for data encryption options
    /// Provides player data security capabilities
    /// </summary>
    public interface IEncryptionOption
    {
        /// <summary>
        /// Encrypt data
        /// </summary>
        /// <param name="data">Data to encrypt</param>
        /// <returns>Encrypted data</returns>
        byte[] Encrypt(byte[] data);

        /// <summary>
        /// Decrypt data
        /// </summary>
        /// <param name="data">Data to decrypt</param>
        /// <returns>Decrypted data</returns>
        byte[] Decrypt(byte[] data);
    }
}