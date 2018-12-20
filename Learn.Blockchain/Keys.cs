using System;
using System.Security.Cryptography;

namespace Learn.Blockchain
{
    public struct Keys
    {
        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public Keys(PrivateKey privateKey, PublicKey publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public static Keys Create()
        {
            using (CngKey cngKey = CngKey.Create(CngAlgorithm.ECDsaP521, null, new CngKeyCreationParameters()
            {
                ExportPolicy = CngExportPolicies.AllowPlaintextExport
            }))
            {
                return new Keys(
                    privateKey: new PrivateKey(cngKey.Export(CngKeyBlobFormat.EccPrivateBlob)),
                    publicKey: new PublicKey(cngKey.Export(CngKeyBlobFormat.EccPublicBlob)));
            }
        }
    }
}
