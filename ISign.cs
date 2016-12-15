using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryproLib
{
    public interface ISign
    {
        void Sign(Stream input, Stream output);
        void SetKey(Stream key);
        void SetHashFunction(IHash hash);
        bool Verify(Stream input);
    }
}
