
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace CryproLib
{
    public class GOSTLib
    {
        public class GostProvider : ICryptoAlgorithm 
        {
            public void EncryptData(Stream input, Stream output)
            {
                GOST28147_89 gost = new GOST28147_89();
                gost.SetOptimTable();

                byte[] encryptedData = new byte[4];
                int cnt = 0;

                input.Position = 0;
                while ((cnt = input.Read(encryptedData, 0, 4)) != 0)
                {
                    for (; cnt < 4; ++cnt)
                    {
                        encryptedData[cnt] = 0;
                    }
                    encryptedData = gost.GOSTDataCrypt(encryptedData);        
                    output.Write(encryptedData, 0, encryptedData.Length);
                    encryptedData = new byte[4];
                }
            }
            public void DecryptData(Stream input, Stream output) 
            {
                GOST28147_89 gost = new GOST28147_89();
                gost.SetOptimTable();

                byte[] decryptedData = new byte[8];
                int cnt = 0;

                input.Position = 0;
                while ((cnt = input.Read(decryptedData, 0, 8)) != 0)
                {
                    decryptedData = gost.GOSTDataDecrypt(decryptedData);      
               
                    output.Write(decryptedData, 0, decryptedData.Length);
                    decryptedData = new byte[8];
                }
            }

            public void SetKey(Stream algorithm)
            {
                throw new NotImplementedException();
            }
        }
        public class GOST28147_89 : ICryptoAlgorithm
        {
            private uint[] key = new uint[8] { 0x11111111, 0x22222222, 0x33333333, 0x44444444, 0x55555555, 0x66666666, 0x77777777, 0x88888888 };

            private static byte[] h8 = { 0x14, 0x04, 0x13, 0x01, 0x02, 0x15, 0x11, 0x08, 0x03, 0x10, 0x06, 0x12, 0x05, 0x09, 0x00, 0x07 };
            private static byte[] h7 = { 0x15, 0x01, 0x08, 0x14, 0x06, 0x11, 0x03, 0x04, 0x09, 0x07, 0x02, 0x13, 0x12, 0x00, 0x05, 0x10 };
            private static byte[] h6 = { 0x10, 0x00, 0x09, 0x14, 0x06, 0x03, 0x15, 0x05, 0x01, 0x13, 0x12, 0x07, 0x11, 0x04, 0x02, 0x08 };
            private static byte[] h5 = { 0x07, 0x13, 0x14, 0x03, 0x00, 0x06, 0x09, 0x10, 0x01, 0x02, 0x08, 0x05, 0x11, 0x12, 0x04, 0x15 };
            private static byte[] h4 = { 0x02, 0x12, 0x04, 0x01, 0x07, 0x10, 0x11, 0x06, 0x08, 0x05, 0x03, 0x15, 0x13, 0x00, 0x14, 0x09 };
            private static byte[] h3 = { 0x12, 0x01, 0x10, 0x15, 0x09, 0x02, 0x06, 0x08, 0x00, 0x13, 0x03, 0x04, 0x14, 0x07, 0x05, 0x11 };
            private static byte[] h2 = { 0x04, 0x11, 0x02, 0x14, 0x15, 0x00, 0x08, 0x13, 0x03, 0x12, 0x09, 0x07, 0x05, 0x10, 0x06, 0x01 };
            private static byte[] h1 = { 0x13, 0x02, 0x08, 0x04, 0x06, 0x15, 0x11, 0x01, 0x10, 0x09, 0x03, 0x14, 0x05, 0x00, 0x12, 0x07 };

            private byte[] h87 = new byte[256];
            private byte[] h65 = new byte[256];
            private byte[] h43 = new byte[256];
            private byte[] h21 = new byte[256];

            private static int bytesNumber = 4;

            public void EncryptData(Stream input, Stream output)
            {
                GOST28147_89 gost = new GOST28147_89();
                gost.SetOptimTable();

                byte[] encryptedData = new byte[4];
                int cnt = 0;

                input.Position = 0;
                while ((cnt = input.Read(encryptedData, 0, 4)) != 0)
                {
                    for (; cnt < 4; ++cnt)
                    {
                        encryptedData[cnt] = 0;
                    }
                    //if (cnt < 4)
                    encryptedData = gost.GOSTDataCrypt(encryptedData);        
                    output.Write(encryptedData, 0, encryptedData.Length);
                    encryptedData = new byte[4];
                }
                
            }
            public void DecryptData(Stream input, Stream output)
            {
                GOST28147_89 gost = new GOST28147_89();
                gost.SetOptimTable();

                byte[] decryptedData = new byte[8];
                int cnt = 0;

                input.Position = 0;
                while ((cnt = input.Read(decryptedData, 0, 8)) != 0)
                {
                    decryptedData = gost.GOSTDataDecrypt(decryptedData);        
                    output.Write(decryptedData, 0, decryptedData.Length);
                    decryptedData = new byte[8];
                }
            }

            public void SetKey(Stream algorithm)
            {
                throw new NotImplementedException();
            }

            public void SetOptimTable()
            {
                for (int i = 0; i < 256; i++)
                {
                    h87[i] = (byte)(h8[i >> 4] << 4 | h7[i & 15]);
                    h65[i] = (byte)(h6[i >> 4] << 4 | h5[i & 15]);
                    h43[i] = (byte)(h4[i >> 4] << 4 | h3[i & 15]);
                    h21[i] = (byte)(h2[i >> 4] << 4 | h1[i & 15]);
                }
            }


            private uint ReplaceWithTable(uint x)
            {
                x = (uint) (h87[x >> 24 & 255] << 24 | h65[x >> 16 & 255] << 16 |
                        h43[x >> 8 & 255] << 8 | h21[x & 255]);

                return x << 11 | x >> (32 - 11);
            }

            public uint ByteArrToUint(byte[] data)
            {
                uint result = 0;
                for (int i = 0; i < data.Length; ++i)
                    result = (uint)(result | (data[i] << (8 * i)));

                return result;
            }

            public void UintToByteArr(int start, int length, byte[] destination, uint source)
            {
                for (int i = 0; i < length; ++i)
                {
                    destination[start + i] = (byte)((source & (0x000000FF << 8 * i)) >> 8 * i);
                }
            }

            public void GOSTBlockCrypt(uint[] planeDataBlock, uint[] encryptedDataBlck, uint[] key)
            {
                uint n1, n2;

                n1 = planeDataBlock[0];
                n2 = planeDataBlock[1];

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 7; j += 2)
                    {
                        n2 ^= ReplaceWithTable(n1 + key[j]);
                        n1 ^= ReplaceWithTable(n2 + key[j + 1]);
                    }
                }

                for (int i = 7; i > 0; i -= 2)
                {
                    n2 ^= ReplaceWithTable(n1 + key[i]);
                    n1 ^= ReplaceWithTable(n2 + key[i - 1]);
                }

                encryptedDataBlck[0] = n2;
                encryptedDataBlck[1] = n1;
            }

            public void GOSTBlockDecrypt(uint[] encryptedDataBlock, uint[] decryptedDataBlock, uint[] key)
            {
                uint n1, n2;

                n1 = encryptedDataBlock[0];
                n2 = encryptedDataBlock[1];

                for (int i = 0; i < 7; i += 2)
                {
                    n2 ^= ReplaceWithTable(n1 + key[i]);
                    n1 ^= ReplaceWithTable(n2 + key[i + 1]);
                }

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 7; j > 0; j -= 2)
                    {
                        n2 ^= ReplaceWithTable(n1 + key[j]);
                        n1 ^= ReplaceWithTable(n2 + key[j - 1]);
                    }
                }

                decryptedDataBlock[0] = n2;
                decryptedDataBlock[1] = n1;
            }

            private void PlaneDataResize(ref byte[] planeData)
            {
                if (planeData.Length % 4 != 0)
                {
                    int lenght = (planeData.Length / 4) * 4 + 4;
                    Array.Resize<byte>(ref planeData, lenght);
                }

                if ((planeData.Length / 4) % 2 != 0)
                    Array.Resize<byte>(ref planeData, planeData.Length + 4);
            }

            public byte[] GOSTDataCrypt(byte[] planeData)
            {
                byte[] tmpByteArrData = new byte[bytesNumber];
                byte[] cryptedByteArrData;
                uint[] tmpUintData = new uint[2];
                uint[] cryptedUintData = new uint[2];
                int k = 0;
                int j = 0;
                int p = 0;

                PlaneDataResize(ref planeData);
                cryptedByteArrData = new byte[planeData.Length];

                for (int i = 0; i < planeData.Length; ++i)
                {
                    tmpByteArrData[j] = planeData[i];
                    if (j == 3)
                    {
                        j = 0;
                        tmpUintData[k] = ByteArrToUint(tmpByteArrData);
                        ++k;
                        if (k == 2)
                        {
                            k = 0;
                            
                            GOSTBlockCrypt(tmpUintData, cryptedUintData, key);

                            for (int m = 0; m < cryptedUintData.Length; ++m)
                            {
                                UintToByteArr(p, bytesNumber, cryptedByteArrData, cryptedUintData[m]);
                                p += bytesNumber;
                            }
                        }
                    }
                    else
                    {
                        ++j;
                    }
                }
                return cryptedByteArrData;
            }
            private void EncryptedDataResize(ref byte[] encryptedData)
            {
                if (encryptedData.Length % 4 != 0)
                {
                    int lenght = (encryptedData.Length / 5) * 4 + 4;
                    Array.Resize<byte>(ref encryptedData, lenght);
                }
            }
            public byte[] GOSTDataDecrypt(byte[] encryptedData)
            {
                byte[] tmpByteArrData = new byte[bytesNumber];
                byte[] decryptedByteArrData;
                uint[] tmpUintData = new uint[2];
                uint[] decryptedUintData = new uint[2];
                int k = 0;
                int j = 0;
                int p = 0;

                EncryptedDataResize(ref encryptedData);
                decryptedByteArrData = new byte[encryptedData.Length];

                for (int i = 0; i < encryptedData.Length; ++i)
                {
                    tmpByteArrData[j] = encryptedData[i];
                    if (j == 3)
                    {
                        j = 0;
                        tmpUintData[k] = ByteArrToUint(tmpByteArrData);
                        ++k;
                        if (k == 2)
                        {
                            k = 0;
                           
                            GOSTBlockDecrypt(tmpUintData, decryptedUintData, key);

                            for (int m = 0; m < decryptedUintData.Length; ++m)
                            {
                                UintToByteArr(p, bytesNumber, decryptedByteArrData, decryptedUintData[m]);
                                p += bytesNumber;
                            }
                        }
                    }
                    else
                    {
                        ++j;
                    }
                }

                tmpByteArrData = new byte[4];
                for (int i = 0; i < 4; ++i)
                {
                    tmpByteArrData[i] = decryptedByteArrData[i];
                }
                decryptedByteArrData = tmpByteArrData;
                return decryptedByteArrData;
            }
        }
    }
}
