﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace LegendCR.Helpers
{
    public class EncryptionHelper_OLD
    {
        public static string Encrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "DutWPFmEi%252fjVu8IrVPPl7A%253d%253d";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }

                return HttpUtility.UrlEncode(clearText);
            }
            catch (Exception)
            {

                return null;
            }

        }
        public static string Decrypt(string cipherText)
        {
            try
            {
                var UrlDecode = HttpUtility.UrlDecode(cipherText);
                string EncryptionKey = "DutWPFmEi%252fjVu8IrVPPl7A%253d%253d";
                byte[] cipherBytes = Convert.FromBase64String(UrlDecode);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        UrlDecode = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return UrlDecode;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}