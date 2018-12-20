using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Blockchain
{
    public struct PrivateKey
    {
        private readonly byte[] _key;

        public PrivateKey(byte[] key)
        {
            if (key?.Any() != true) throw new ArgumentException("Key is empty", nameof(key));

            _key = key;
        }

        public static implicit operator byte[] (PrivateKey privateKey)
            => privateKey._key;
    }
}
