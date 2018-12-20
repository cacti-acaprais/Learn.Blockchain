using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public static class DocumentConverter
    {
        public static DocumentConverter<string> STRING
            => new DocumentConverter<string>(
                document =>
                {
                    if (string.IsNullOrEmpty(document)) throw new ArgumentNullException(nameof(document));
                    return new Document(Encoding.UTF8.GetBytes(document));
                },
                document =>
                {
                    byte[] documentBytes = document;
                    return Encoding.UTF8.GetString(documentBytes);
                });

        public static DocumentConverter<string> BASE64
            => new DocumentConverter<string>(
                document =>
                {
                    if (string.IsNullOrEmpty(document)) throw new ArgumentNullException(nameof(document));
                    return new Document(System.Convert.FromBase64String(document));
                },
                document =>
                {
                    byte[] documentBytes = document;
                    return System.Convert.ToBase64String(documentBytes);
                });

        public static DocumentConverter<byte[]> JPEG
            => throw new NotImplementedException();
    }

    public class DocumentConverter<T>
    {
        private readonly Func<T, Document> _getDocument;
        private readonly Func<Document, T> _getT;

        public DocumentConverter(Func<T, Document> getDocument, Func<Document, T> getT)
        {
            _getDocument = getDocument ?? throw new ArgumentNullException(nameof(getDocument));
            _getT = getT ?? throw new ArgumentNullException(nameof(getT));
        }

        public Document Get(T document)
            => _getDocument(document);

        public T Get(Document document)
            => _getT(document);
    }
}
