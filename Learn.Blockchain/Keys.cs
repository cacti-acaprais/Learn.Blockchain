using System;
using System.Security.Cryptography;

namespace Learn.Blockchain
{
    public struct Keys
    {
        public byte[] PrivateKey { get; }
        public byte[] PublicKey { get; }

        public Keys(byte[] privateKey, byte[] publicKey)
        {
            PrivateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        }

        public static Keys Create()
        {
            using (CngKey cngKey = CngKey.Create(CngAlgorithm.ECDsaP521, null, new CngKeyCreationParameters()
            {
                ExportPolicy = CngExportPolicies.AllowPlaintextExport
            }))
            {
                return new Keys(
                    privateKey: cngKey.Export(CngKeyBlobFormat.EccPrivateBlob),
                    publicKey: cngKey.Export(CngKeyBlobFormat.EccPublicBlob));
            }
        }
    }
}
