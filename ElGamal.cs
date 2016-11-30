using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CryproLib
{
    public class ElGamal : ICryptoAlgorithm
    {
        static int p = 257;
        static int x = 77;
        static int g = 2;
        static int y;
        public void DecryptData(Stream input, Stream output)
        {
            byte a;
            byte b;
            byte[] bytesToDecrypt = new byte[2];

            input.Position = 0;
            for (; input.Position < input.Length ;)
            {
                input.Read(bytesToDecrypt, 0, 2);

                a = bytesToDecrypt[0];
                b = bytesToDecrypt[1];

                int deM = Multiple(b, Power(a, p - 1 - x, p), p);// m=b*(a^x)^(-1)mod p =b*a^(p-1-x)mod p 
                output.WriteByte((byte)deM);
            }
        }
        public void EncryptData(Stream input, Stream output)
        {
            byte byteToCrypt;
          
            y = Power(g, x, p);

            input.Position = 0;
            for (; input.Position < input.Length ;)
            {
                byteToCrypt = (byte)input.ReadByte();
                
                int k = Rand() % (p - 2) + 1; // 1 < k < (p-1)
                int a = Power(g, k, p);
                int b = Multiple(Power(y, k, p), byteToCrypt, p);
               
                output.WriteByte((byte)a);
                output.WriteByte((byte)b);


            }
        }

        public void SetKey(Stream key)
        {
            byte[] buf = new byte[key.Length];
            key.Read(buf, 0, buf.Length);
            string[] keys = buf.ToString().Split(' ');
            try
            {
                p = Convert.ToInt32(keys[0]);
                g = Convert.ToInt32(keys[1]);
                x = Convert.ToInt32(keys[2]);
            } 
            catch (Exception ex)
            { };

        }

        private int Rand()
        {
            Random random = new Random();
            return random.Next();
        }
        int Power(int a, int b, int m) // a^b mod m
        {
            int tmp = a;
            int sum = tmp;
            for (int i = 1; i < b; i++)
            {
                for (int j = 1; j < a; j++)
                {
                    sum += tmp;
                    if (sum >= m)
                    {
                        sum -= m;
                    }
                }
                tmp = sum;
            }
            return tmp;
        }
        int Multiple(int a, int b, int m) // a*b mod m
        {
            int sum = 0;
            for (int i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= m)
                {
                    sum -= m;
                }
            }
            return sum;
        }
    }
}

