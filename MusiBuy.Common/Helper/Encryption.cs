using System.Security.Cryptography;
using System.Text;


namespace MusiBuy.Common.Helpers
{
    public class Encryption
    {
        protected static string strKey = "OTCPasswordKeyForLiveOn01APR2024";

        public static string EncryptWithUrlEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(Encrypt(str));
        }
        public static string DecryptWithUrlDecode(string str)
        {
            return Decrypt(System.Web.HttpUtility.UrlDecode(str));
        }

        public static string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(strKey);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }
            catch (Exception e)
            {
                return plainText;
            }
        }

        public static string Decrypt(string cipherText)
        {

            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText.Replace(' ', '+'));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(strKey);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return cipherText;
            }
        }
    }
}
