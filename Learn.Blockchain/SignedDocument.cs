using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Learn.Blockchain
{
    public class SignedDocument : ISignedDocument
    {
        public byte[] Signature{ get; }
        public byte[] PublicKey { get; }
        public byte[] Document { get; }

        public SignedDocument(byte[] document, byte[] publicKey, byte[] signature)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        public static SignedDocument Create(Keys keys, byte[] document)
        {
            using (CngKey privateCngKey = CngKey.Import(keys.PrivateKey, CngKeyBlobFormat.EccPrivateBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                byte[] signature = dsa.SignData(document);
                return new SignedDocument(document, keys.PublicKey, signature);
            }
        }

        public bool Verify()
        {
            //Some optimisations can be made to keep dsa in cache.
            using (CngKey privateCngKey = CngKey.Import(PublicKey, CngKeyBlobFormat.EccPublicBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                return dsa.VerifyData(Document, Signature);
            }
        }
    }
}
