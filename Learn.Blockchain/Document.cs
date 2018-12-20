using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Blockchain
{
    public struct Document
    {
        private readonly byte[] _document;

        public Document(byte[] document)
        {
            if (document?.Any() != true) throw new ArgumentException("Document is empty.", nameof(document));

            _document = document;
        }

        public static implicit operator byte[](Document document)
            => document._document;
    }
}
