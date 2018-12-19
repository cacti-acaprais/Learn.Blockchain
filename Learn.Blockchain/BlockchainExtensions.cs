using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public static class BlockchainExtensions
    {
        public static ISignedTransaction ToSignedTransactionRoot(this ISignedDocument signedDocument)
            => new SignedTransactionRoot(signedDocument);

        public static ISignedTransaction Add(this ISignedTransaction signedTransaction, Keys keys, byte[] document)
            => SignedTransaction.Create(keys, document, signedTransaction);

        public static IEnumerable<ISignedDocument> ToEnumerable(this ISignedTransaction signedTransaction)
            => new EnumerableSignedTransaction(signedTransaction);

        private class EnumerableSignedTransaction : IEnumerable<ISignedDocument>
        {
            private readonly ISignedTransaction _signedTransaction;

            public EnumerableSignedTransaction(ISignedTransaction signedTransaction)
            {
                _signedTransaction = signedTransaction ?? throw new ArgumentNullException(nameof(signedTransaction));
            }

            public IEnumerator<ISignedDocument> GetEnumerator()
                => new Enumerator(_signedTransaction);

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            private class Enumerator : IEnumerator<ISignedDocument>
            {
                private readonly ISignedTransaction _firstSignedTransaction;
                private ISignedTransaction _current;

                public ISignedDocument Current 
                    => _current;

                object IEnumerator.Current 
                    => _current;

                public Enumerator(ISignedTransaction signedTransaction)
                {
                    _firstSignedTransaction = signedTransaction ?? throw new ArgumentNullException(nameof(signedTransaction));
                }

                public void Dispose()
                {
                    
                }

                public bool MoveNext()
                {
                    if(_current == null)
                    {
                        _current = _firstSignedTransaction;
                        return true;
                    }

                    if (_current.PreviousSignedTransaction == null)
                        return false;

                    _current = _current.PreviousSignedTransaction;

                    return true;
                }

                public void Reset()
                {
                    _current = null;
                }
            }
        }
    }
}
