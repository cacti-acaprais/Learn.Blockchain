using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Learn.Blockchain
{
    public class SignedDocument : ISignedDocument
    {
        public Signature Signature{ get; }
        public PublicKey PublicKey { get; }
        public Document Document { get; }

        public SignedDocument(Document document, PublicKey publicKey, Signature signature)
        {
            Document = document;
            PublicKey = publicKey;
            Signature = signature;
        }

        public static SignedDocument Create(Keys keys, Document document)
        {
            using (CngKey privateCngKey = CngKey.Import(keys.PrivateKey, CngKeyBlobFormat.EccPrivateBlob))
            using (ECDsaCng dsa = new ECDsaCng(privateCngKey))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                Signature signature = new Signature(dsa.SignData(document));
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
