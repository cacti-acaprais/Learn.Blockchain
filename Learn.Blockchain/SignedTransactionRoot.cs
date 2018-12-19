using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public class SignedTransactionRoot : ISignedTransaction
    {
        private readonly ISignedDocument _signedDocument;

        public SignedTransactionRoot(ISignedDocument signedDocument)
        {
            _signedDocument = signedDocument ?? throw new ArgumentNullException(nameof(signedDocument));
        }

        public ISignedTransaction PreviousSignedTransaction 
            => null;

        public byte[] Document 
            => _signedDocument.Document;

        public byte[] PublicKey 
            => _signedDocument.PublicKey;

        public byte[] Signature 
            => _signedDocument.Signature;

        public bool Verify()
            => _signedDocument.Verify();
    }
}
