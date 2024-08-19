using System.Security.Cryptography;
using System.Text;

namespace RSAwebAPI.Services
{
    public class RsaServices
    {
        public void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            }
        }

        public byte[] Encrypt(string text, string publicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                return rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1);
            }
        }

        public string Decrypt(byte[] encryptedData, string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                return Encoding.UTF8.GetString(rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1));
            }
        }
    }
}
