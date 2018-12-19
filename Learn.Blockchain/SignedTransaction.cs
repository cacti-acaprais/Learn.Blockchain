using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Learn.Blockchain
{
    public class SignedTransaction : ISignedTransaction
    {
        public ISignedTransaction PreviousSignedTransaction { get; }

        public byte[] Document { get; }

        public byte[] PublicKey { get; }

        public byte[] Signature { get; }

        public SignedTransaction(ISignedTransaction previousSignedDocument, byte[] document, byte[] publicKey, byte[] signature)
        {
            PreviousSignedTransaction = previousSignedDocument ?? throw new ArgumentNullException(nameof(previousSignedDocument));
            Document = document ?? throw new ArgumentNullException(nameof(document));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        public static SignedTransaction Create(Keys keys, byte[] document, ISignedTransaction previousSignedDocument)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (previousSignedDocument == null) throw new ArgumentNullException(nameof(previousSignedDocument));
            if (!previousSignedDocument.Verify()) throw new ArgumentException("Previous signature is not valid.", nameof(previousSignedDocument));

            byte[] hash = document
                .Concat(previousSignedDocument.Signature)
                .ToArray();

            using (CngKey privateCngKey = CngKey.Import(keys.PrivateKey, CngKeyBlobFormat.EccPrivateBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                byte[] signature = dsa.SignData(hash);
                return new SignedTransaction(previousSignedDocument, document, keys.PublicKey, signature);
            }
        }

        public bool Verify()
        {
            using (CngKey privateCngKey = CngKey.Import(PublicKey, CngKeyBlobFormat.EccPublicBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                byte[] hash = Document
                    .Concat(PreviousSignedTransaction.Signature)
                    .ToArray();

                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                return dsa.VerifyData(hash, Signature) && PreviousSignedTransaction.Verify();
            }
        }
    }
}