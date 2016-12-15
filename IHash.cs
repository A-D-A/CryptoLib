using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryproLib
{
    public interface IHash
    {
        void GetHash(Stream input, Stream output);

    }
}
