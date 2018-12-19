using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public interface ISignedTransaction : ISignedDocument
    {
        /// <summary>
        /// Previous transaction part. Can be null if root transaction.
        /// </summary>
        ISignedTransaction PreviousSignedTransaction { get; }
    }
}
