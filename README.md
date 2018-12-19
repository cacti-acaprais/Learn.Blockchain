# Learn.Blockchain
Experiments arount Blockchains helping to understand how it works. It's Based on System.Security.Cryptography.Cng.

- Keys : create and holds public and private keys.
- SignedDocument : sign a document with Keys. A signedDocument can be converted to a SignedTransaction (the genesis block of the chain).
- SignedTransaction : link documents in a signed transactions. If a document in the transaction chain is invalid, the transaction is invalid. A SignedTransaction can be converted to an IEnumerable of SignedDocument in order to extract the document through Linq or a foreach.
