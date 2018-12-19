# Learn.Blockchain
Try to understand how it works

Based on System.Security.Cryptography.Cng.

Keys : create and holds public and private keys.

SignedDocument : sign a document with Keys.

SignedTransaction : link documents in a signed transactions. If a document in the transaction chain is invalid, the transaction is invalid. A SignedTransaction can be converted to an IEnumerable of SignedDocument in order to extract the document through Linq or a foreach.
