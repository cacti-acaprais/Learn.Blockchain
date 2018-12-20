using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Blockchain
{
    public struct PublicKey
    {
        private readonly byte[] _key;

        public PublicKey(byte[] key)
        {
            if (key?.Any() != true) throw new ArgumentException("Key is empty", nameof(key));

            _key = key;
        }

        public static implicit operator byte[](PublicKey publicKey)
            => publicKey._key;
    }
}
