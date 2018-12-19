using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public interface ISignedDocument
    {
        byte[] Document { get; }
        byte[] PublicKey { get; }
        byte[] Signature { get; }

        bool Verify();
    }
}
