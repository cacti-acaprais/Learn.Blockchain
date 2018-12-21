using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Learn.Blockchain.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GenerateKeys()
        {
            Keys keys = Keys.Create();
            Assert.IsNotNull(keys);

            byte[] publicKey = keys.PublicKey;
            byte[] privateKey = keys.PrivateKey;

            Assert.IsTrue(publicKey.Any());
            Assert.IsTrue(privateKey.Any());
        }

        [TestMethod]
        public void SignedDocumentValid()
        {
            Keys keys = Keys.Create();
            SignedDocument signedDocument = SignedDocument.Create(keys, DocumentConverter.STRING.Get("It's a test with a basic document."));
            Assert.IsTrue(signedDocument.Verify());
        }

        [TestMethod]
        public void CorruptedDocument()
        {
            Keys keys = Keys.Create();
            SignedDocument signedDocument = SignedDocument.Create(keys, DocumentConverter.STRING.Get("It's a test with a basic document."));

            var corruptedDocument = new SignedDocument(
                document: DocumentConverter.STRING.Get("It's a test with a different document."),
                publicKey: signedDocument.PublicKey, 
                signature: signedDocument.Signature);

            Assert.IsTrue(signedDocument.Verify());
            Assert.IsFalse(corruptedDocument.Verify());
        }

        [TestMethod]
        public void CorruptedUser()
        {
            Keys keys = Keys.Create();
            ISignedDocument signedDocument = SignedDocument.Create(keys, DocumentConverter.STRING.Get("It's a test with a basic document."));

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
            Keys user1keys = Keys.Create();
            Keys user2Keys = Keys.Create();

            ISignedTransaction signedTransaction = SignedDocument.Create(user1keys, DocumentConverter.STRING.Get("It's a test with a basic document."))
                .ToSignedTransactionRoot()
                .Add(user1keys, DocumentConverter.STRING.Get("Updated."))
                .Add(user2Keys, DocumentConverter.STRING.Get("Add a signature to the document."));

            Assert.IsTrue(signedTransaction.Verify());
        }

        [TestMethod]
        public void CorruptedTransactions()
        {
            Keys user1keys = Keys.Create();
            Keys user2Keys = Keys.Create();
            ISignedDocument signedDocument = SignedDocument.Create(user1keys, DocumentConverter.STRING.Get("It's a test with a basic document."));

            ISignedTransaction signedTransaction1 = signedDocument
                .ToSignedTransactionRoot()
                .Add(user1keys, DocumentConverter.STRING.Get("Updated."));

            ISignedTransaction signedTransaction2 = signedTransaction1
                .Add(user2Keys, DocumentConverter.STRING.Get("Add a signature to the document."));

            ISignedTransaction corruptedTransaction = new SignedTransaction(signedTransaction1.PreviousSignedTransaction, DocumentConverter.STRING.Get("Replace with a corrupted document"), signedTransaction1.PublicKey, signedTransaction1.Signature);
            signedTransaction2 = new SignedTransaction(corruptedTransaction, signedTransaction2.Document, signedTransaction2.PublicKey, signedTransaction2.Signature);

            Assert.IsFalse(signedTransaction2.Verify());
        }

        [TestMethod]
        public void ExtractSignedDocuments()
        {
            string fistDocumentString = "It's a test with a basic document.";

            Keys user1keys = Keys.Create();
            Keys user2Keys = Keys.Create();

            IEnumerable<ISignedDocument> signedDocuments = SignedDocument.Create(user1keys, DocumentConverter.STRING.Get(fistDocumentString))
                .ToSignedTransactionRoot()
                .Add(user1keys, DocumentConverter.STRING.Get("Updated."))
                .Add(user2Keys, DocumentConverter.STRING.Get("Add a signature to the document."))
                .ToEnumerable();

            string[] documentStrings = signedDocuments
                .Select(x => DocumentConverter.STRING.Get(x.Document))
                .ToArray();

            Assert.IsTrue(signedDocuments.All(x => x.Verify()));
            Assert.IsTrue(documentStrings.Count() == 3);
            Assert.AreEqual(fistDocumentString, documentStrings.Last());
        }

        [TestMethod]
        public void ExtractDocumentsOfRoot()
        {
            string documentString = "It's a test with a basic document.";

            Keys user1keys = Keys.Create();
            IEnumerable<ISignedDocument> signedDocuments = SignedDocument.Create(user1keys, DocumentConverter.STRING.Get(documentString))
                .ToSignedTransactionRoot()
                .ToEnumerable();

            string[] documentStrings = signedDocuments
                .Select(x => DocumentConverter.STRING.Get(x.Document))
                .ToArray();

            Assert.IsTrue(signedDocuments.All(x => x.Verify()));
            Assert.IsTrue(documentStrings.Count() == 1);
            Assert.AreEqual(documentString, documentStrings.Single());
        }

        [TestMethod]
        public void ConvertDocument()
        {
            string message = "It's a test with a basic document.";

            Document document = DocumentConverter.STRING.Get(message);
            string base64Document = DocumentConverter.BASE64.Get(document);
            Document fromBase64Document = DocumentConverter.BASE64.Get(base64Document);
            string stringDocument = DocumentConverter.STRING.Get(fromBase64Document);

            Assert.AreEqual(message, stringDocument);
        }

        [TestMethod]
        public void ComposedConversion()
        {
            string message = "It's à tèst with a basic document.";

            //Convert a string document to a base64 document
            DocumentConverter<string> documentConverter = DocumentConverter
                .STRING
                .Compose(DocumentConverter.BASE64);

            Document document = documentConverter.Get(message);
            string stringDocument = documentConverter.Get(document);

            Assert.AreEqual(message, stringDocument);
        }
    }
}
