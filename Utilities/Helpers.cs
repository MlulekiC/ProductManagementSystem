using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class Helpers
    {
        private const string ConstantKey = "A2B2C3D4E5F6G7H8I9J0K1L2M3N4O5P6";

        #region Encryption
        public static string Encrypt(string plainText)
        {
            // Convert string key to bytes
            byte[] key = Encoding.UTF8.GetBytes(ConstantKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            byte[] key = Encoding.UTF8.GetBytes(ConstantKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        #endregion Encryption

        #region Pagination
        public class PaginatedList<T> : List<T>
        {
            public int PageNumber { get; private set; }
            public int TotalPages { get; private set; }

            public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
            {
                PageNumber = pageNumber;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                this.AddRange(items);
            }

            public bool HasPreviousPage => PageNumber > 1;
            public bool HasNextPage => PageNumber < TotalPages;

            public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
            {
                var count = await source.CountAsync();
                var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PaginatedList<T>(items, count, pageNumber, pageSize);
            }
        }
        #endregion Pagination
    }
}
