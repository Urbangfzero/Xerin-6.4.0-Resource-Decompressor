using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Core
{
    public static class Decryptor
    {
        public static bool Decrypt(byte[] input, int keySeed, out byte[] result)
        {
            result = null;

            try
            {
                Logger.Info("Decrypting...");

                byte[] key = SHA256.Create().ComputeHash(BitConverter.GetBytes(keySeed));

                byte[] iv = new byte[16];
                Buffer.BlockCopy(input, 0, iv, 0, 16);

                byte[] hmac = new byte[32];
                Buffer.BlockCopy(input, 16, hmac, 0, 32);

                byte[] cipher = new byte[input.Length - 48];
                Buffer.BlockCopy(input, 48, cipher, 0, cipher.Length);

                using (var hmacsha = new HMACSHA256(key))
                {
                    if (!hmacsha.ComputeHash(cipher).SequenceEqual(hmac))
                        throw new Exception("HMAC mismatch");
                }

                byte[] decrypted;

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipher, 0, cipher.Length);
                        cs.FlushFinalBlock();
                        decrypted = ms.ToArray();
                    }
                }

                Random rnd = new Random(keySeed);
                byte[] xorKey = new byte[32];
                rnd.NextBytes(xorKey);

                for (int i = 0; i < decrypted.Length; i++)
                    decrypted[i] ^= xorKey[i % xorKey.Length];

                result = decrypted;
                Logger.Success("Decryption done.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }
    }
}