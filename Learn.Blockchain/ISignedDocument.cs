using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public interface ISignedDocument
    {
        Document Document { get; }
        PublicKey PublicKey { get; }
        Signature Signature { get; }

        bool Verify();
    }
}
