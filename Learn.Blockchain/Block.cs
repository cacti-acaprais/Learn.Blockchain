using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Learn.Blockchain
{
    public class Block
    {
        public DateTimeOffset TimeStamp { get; }
        public string PreviousHash { get; }
        public string Hash { get; }
        public object Data { get; }

        protected Block(DateTimeOffset timeStamp, object data, string previousHash)
        {
            TimeStamp = timeStamp;
            Data = data;
            PreviousHash = previousHash;
            Hash = GetHash(timeStamp, data, previousHash);
        }

        protected static string GetHash(DateTimeOffset timeStamp, object data, string previousHash)
        {
            SHA256 sha256 = SHA256.Create();
            string jsonData = JsonConvert.SerializeObject(data);
            byte[] dataBytes = Encoding.UTF8.GetBytes($"{timeStamp}-{previousHash}-{jsonData}");
            byte[] hashBytes = sha256.ComputeHash(dataBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public bool IsValid()
        {
            return Hash == GetHash(TimeStamp, Data, PreviousHash);
        }
    }

    public class Block<T> : Block
    {
        public new T Data
        {
            get => (T)base.Data;
        }

        public static Block<T> Create(DateTimeOffset timeStamp, T data)
        {
            return new Block<T>(timeStamp, data, string.Empty);
        }

        public static Block<T> Create(DateTimeOffset timeStamp, T data, Block previousBlock)
        {
            return new Block<T>(timeStamp, data, previousBlock.Hash);
        }

        protected Block(DateTimeOffset timeStamp, T data, string previousHash)
            : base(timeStamp, data, previousHash)
        {

        }
    }
        
}
