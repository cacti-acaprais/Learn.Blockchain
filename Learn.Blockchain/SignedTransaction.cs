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

        public Document Document { get; }

        public PublicKey PublicKey { get; }

        public Signature Signature { get; }

        public SignedTransaction(ISignedTransaction previousSignedDocument, Document document, PublicKey publicKey, Signature signature)
        {
            PreviousSignedTransaction = previousSignedDocument ?? throw new ArgumentNullException(nameof(previousSignedDocument));
            Document = document;
            PublicKey = publicKey;
            Signature = signature;
        }

        public static SignedTransaction Create(Keys keys, Document document, ISignedTransaction previousSignedDocument)
        {
            if (previousSignedDocument == null) throw new ArgumentNullException(nameof(previousSignedDocument));
            if (!previousSignedDocument.Verify()) throw new ArgumentException("Previous signature is not valid.", nameof(previousSignedDocument));

            byte[] previousSignature = previousSignedDocument.Signature;
            byte[] documentBytes = document;

            byte[] hash = documentBytes
                .Concat(previousSignature)
                .ToArray();

            using (CngKey privateCngKey = CngKey.Import(keys.PrivateKey, CngKeyBlobFormat.EccPrivateBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                Signature signature = new Signature(dsa.SignData(hash));
                return new SignedTransaction(previousSignedDocument, document, keys.PublicKey, signature);
            }
        }

        public bool Verify()
        {
            using (CngKey privateCngKey = CngKey.Import(PublicKey, CngKeyBlobFormat.EccPublicBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                byte[] documentBytes = Document;
                byte[] previousSignature = PreviousSignedTransaction.Signature;
                byte[] hash = documentBytes
                    .Concat(previousSignature)
                    .ToArray();

                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                return dsa.VerifyData(hash, Signature) && PreviousSignedTransaction.Verify();
            }
        }
    }
}