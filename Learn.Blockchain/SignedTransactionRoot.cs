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

        public Document Document 
            => _signedDocument.Document;

        public PublicKey PublicKey 
            => _signedDocument.PublicKey;

        public Signature Signature 
            => _signedDocument.Signature;

        public bool Verify()
            => _signedDocument.Verify();
    }
}
