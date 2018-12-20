using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Blockchain
{
    public struct Signature
    {
        private readonly byte[] _signature;

        public Signature(byte[] signature)
        {
            if (signature?.Any() != true) throw new ArgumentException("Signature is empty", nameof(signature));

            _signature = signature;
        }

        public static implicit operator byte[](Signature signature)
            => signature._signature;
    }
}
