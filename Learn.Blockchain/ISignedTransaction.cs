using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Blockchain
{
    public interface ISignedTransaction : ISignedDocument
    {
        ISignedDocument PreviousSignedDocument { get; }
    }
}
