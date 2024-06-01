using DicomApp.DAL.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DicomApp.Helpers
{
    public class EncryptionManager
    {


        //var UserID = AuthHelper.GetClaimValue(User, "UserID");
        //var currUser = _context.CommonUser.FirstOrDefault(f => f.Id == UserID);

        //PasswordHasher<CommonUser> hasher = new PasswordHasher<CommonUser>();
        //var result = hasher.HashPassword(currUser, currUser.Password);
        //var result2 = hasher.VerifyHashedPassword(currUser, result, "123456");

        //var test = EncryptionManager.Encrypt("Test String for Enc");
        //var test2 = EncryptionManager.Decrypt(test);


        public const string APPLICATION_KEY = "RED_KEY_02249887";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tk77geji210t99u2");

        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new PasswordDeriveBytes(APPLICATION_KEY, null))
            {
                var keyBytes = password.GetBytes(keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream =
                                new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (cipherText == null)
                cipherText = "";
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var password = new PasswordDeriveBytes(APPLICATION_KEY, null))
            {
                var keyBytes = password.GetBytes(keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream =
                                new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount =
                                    cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
    }
}