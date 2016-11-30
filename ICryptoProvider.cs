using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryproLib
{
    interface ICryptoProvider
    {
        void SetCryptoAlgorithm(ICryptoAlgorithm algorithm);
        ICryptoAlgorithm GetCryptoAlgorithm();
        void Encrypt(Stream input, Stream output);
        void Decrypt(Stream input, Stream output);
    }
}
