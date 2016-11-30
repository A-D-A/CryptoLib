using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryproLib
{
    public interface ICryptoAlgorithm
    {
        void EncryptData(Stream input, Stream output);
        void DecryptData(Stream input, Stream output);
        void SetKey(Stream key);
    }
}
