using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace Learn.Blockchain.Tests
{
    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        public void GenerateKeys()
        {
            Keys keys = Keys.Create();
            Assert.IsNotNull(keys);

            Assert.IsTrue(keys.PublicKey.Any());
            Assert.IsTrue(keys.PrivateKey.Any());
        }

        [TestMethod]
        public void SignedDocumentValid()
        {
            byte[] document = GetDocument("It's a test with a basic document.");

            Keys keys = Keys.Create();
            SignedDocument signedDocument = SignedDocument.Create(keys, document);
            Assert.IsTrue(signedDocument.Verify());
        }

        [TestMethod]
        public void CorruptedDocument()
        {
            byte[] document = GetDocument("It's a test with a basic document.");

            Keys keys = Keys.Create();
            SignedDocument signedDocument = SignedDocument.Create(keys, document);

            var corruptedDocument = new SignedDocument(
                document: GetDocument("It's a test with a different document."),
                publicKey: signedDocument.PublicKey, 
                signature: signedDocument.Signature);

            Assert.IsTrue(signedDocument.Verify());
            Assert.IsFalse(corruptedDocument.Verify());
        }

        [TestMethod]
        public void CorruptedUser()
        {
            byte[] document = GetDocument("It's a test with a basic document.");

            Keys keys = Keys.Create();
            ISignedDocument signedDocument = SignedDocument.Create(keys, document);

            var corruptedDocument = new SignedDocument(
                document: signedDocument.Document,
                publicKey: Keys.Create().PublicKey,
                signature: signedDocument.Signature);

            Assert.IsTrue(signedDocument.Verify());
            Assert.IsFalse(corruptedDocument.Verify());
        }

        [TestMethod]
        public void CreateTransactions()
        {
            byte[] document = GetDocument("It's a test with a basic document.");

            Keys user1keys = Keys.Create();
            ISignedDocument signedDocument = SignedDocument.Create(user1keys, document);
            ISignedDocument signedTransaction1 = SignedTransaction.Create(user1keys, ConcatDocumentWith(signedDocument.Document, "Updated."), signedDocument);

            Keys user2Keys = Keys.Create();
            ISignedDocument signedTransaction2 = SignedTransaction.Create(user2Keys, GetDocument("Add a signature to the document."), signedTransaction1);

            Assert.IsTrue(signedTransaction2.Verify());
        }

        [TestMethod]
        public void CorruptedTransactions()
        {
            byte[] document = GetDocument("It's a test with a basic document.");

            Keys user1keys = Keys.Create();
            ISignedDocument signedDocument = SignedDocument.Create(user1keys, document);
            ISignedTransaction signedTransaction1 = SignedTransaction.Create(user1keys, ConcatDocumentWith(signedDocument.Document, "Updated."), signedDocument);

            Keys user2Keys = Keys.Create();
            ISignedDocument signedTransaction2 = SignedTransaction.Create(user2Keys, GetDocument("Add a signature to the document."), signedTransaction1);

            ISignedTransaction corruptedTransaction = new SignedTransaction(signedTransaction1.PreviousSignedDocument, GetDocument("Replace with a corrupted document"), signedTransaction1.PublicKey, signedTransaction1.Signature);
            signedTransaction2 = new SignedTransaction(corruptedTransaction, signedTransaction2.Document, signedTransaction2.PublicKey, signedTransaction2.Signature);

            Assert.IsFalse(signedTransaction2.Verify());
        }

        private byte[] GetDocument(string documentString)
            => Encoding.UTF8.GetBytes(documentString);

        private byte[] ConcatDocumentWith(byte[] document, string documentString)
            => Encoding.UTF8.GetBytes($"{Encoding.UTF8.GetString(document)} {documentString}");
    }
}
