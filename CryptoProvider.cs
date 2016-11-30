using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace CryproLib
{
    public class CryptoProvider : ICryptoProvider
    {
        ICryptoAlgorithm algorithm;
        public void Decrypt(Stream input, Stream output)
        {
            algorithm.DecryptData(input, output);
        }

        public void Encrypt(Stream input, Stream output)
        {
            algorithm.EncryptData(input, output);
        }

        public ICryptoAlgorithm GetCryptoAlgorithm()
        {
            return algorithm;
        }

        public void SetCryptoAlgorithm(ICryptoAlgorithm algorithm)
        {
            this.algorithm = algorithm;

            //throw new NotImplementedException();
        }
    }
}
