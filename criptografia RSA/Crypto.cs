using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace criptografia_RSA
{
    interface ICrypto
    {
        public byte[] Encrypt(byte[] plainData);

        public byte[] Decrypt(byte[] cipherData);
    }

    public class RSACrypto : ICrypto                    
    {
        public readonly RSACryptoServiceProvider RSA;
        public RSACrypto()
        {
            RSA = new RSACryptoServiceProvider();
            RSA.KeySize = Utils.SelectKeySizes(RSA.LegalKeySizes);
        }

        public byte[] Encrypt(byte[] plainData)
        {
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                     criptografia por RSA                   |");
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                       criptografando...                    |");

            var cipherData = RSA.Encrypt(plainData, false);

            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                   criptografando com sucesso               |");

            return cipherData;
        }

        public byte[] Decrypt(byte[] cipherData)
        {
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                    criptografia por RSA                    |");
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                      descriptografando...                  |");

            var plainData = RSA.Decrypt(cipherData, false);

            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                  descriptografado com sucesso              |");

            return plainData;
        }
        
    }

    public class AESCrypto : ICrypto
    {
        readonly RijndaelManaged myRijndael;
        public AESCrypto()
        {
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                criptografia por Rijndael(AES)              |");
            
            myRijndael = new RijndaelManaged();
            myRijndael.KeySize = Utils.SelectKeySizes(myRijndael.LegalKeySizes);

        }
        public byte[] Encrypt(byte[] plainData)
        {
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                criptografia por Rijndael(AES)              |");
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                       criptografando...                    |");

            var cipherData = Utils.PerformCryptography(myRijndael.CreateEncryptor(), plainData);

            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                   criptografando com sucesso               |");

            return cipherData;
        }

        public byte[] Decrypt(byte[] cipherData)
        {
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                criptografia por Rijndael(AES)              |");
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                      descriptografando...                  |");            

            var plainData = Utils.PerformCryptography(myRijndael.CreateDecryptor(), cipherData);

            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                  descriptografado com sucesso              |");

            return plainData;

        }
       
    }


    internal static class Utils
    {
        public static int SelectKeySizes(KeySizes[] legalKeySizes)
        {
            List<int> keySizes = new List<int>();
            foreach (var sizes in legalKeySizes)
                for (int atual = sizes.MinSize; atual <= sizes.MaxSize; atual += sizes.SkipSize)
                    keySizes.Add(atual);

            Int16 selected;

            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                  Seleção de tamanhos de chave              |");
            Console.WriteLine("| ---------------------------------------------------------- |");
            do
            {
                Console.WriteLine("\ntamanhos válido de sua chave:");
                int varMax = 1;
                foreach (var size in keySizes)
                {
                    Console.WriteLine($"{varMax}) {size} bits");
                    varMax++;
                }
                Console.Write("\nPor favor selecione:");

            }
            while (!(Int16.TryParse(Console.ReadLine(), out selected)
                     && selected > 0 && selected <= keySizes.Count));
            Console.WriteLine("\n");
            return keySizes[selected - 1];


        }

        public static byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }


    }

}
