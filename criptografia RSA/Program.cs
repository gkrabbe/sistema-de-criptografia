using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace criptografia_RSA
{
    class Program
    {
        static readonly Stopwatch watch_c = Stopwatch.StartNew();
        
        private static bool IsContinue( string msg)
        {
            watch_c.Stop();
            Console.Write($"{msg}[s/N]");
            var k= Console.ReadKey().Key != ConsoleKey.S;
            watch_c.Start();
            return k;
        }

        static void Main(string[] args)
        {
            Stopwatch watch_all = Stopwatch.StartNew();
            watch_c.Stop();

            Greeting();
            Console.Write("Digite o texto a ser cifrado:");
            var plainData = Encoding.ASCII.GetBytes(Console.ReadLine());
            watch_c.Restart();

            List<string> libs_names = new List<string>() { "RSA", "AES" };

            Stack<ICrypto> libs = new Stack<ICrypto>();


            var lib = SelectLib(true, libs_names);

            libs.Push(lib);
            var cipherData = lib.Encrypt(plainData);

            do
            {
                lib = SelectLib(false, libs_names);
                libs.Push(lib);

                cipherData = lib.Encrypt(cipherData);
                Console.WriteLine("| ---------------------------------------------------------- |");

            } while (libs_names.Count != 0 && IsContinue("Deseja começar finalizar o processo de criptografia?"));
                       

            Console.WriteLine("\n\n| ---------------------------------------------------------- |");
            Console.WriteLine("|          Iniciando processo de descriprografia...          |");

            var plainData_2 = libs.Pop().Decrypt(cipherData);

            foreach (ICrypto crypto in libs)
                plainData_2 = crypto.Decrypt(plainData_2);

            watch_c.Stop();

            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("|                  Descriptografia concluida!                |");
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine($"\nTempo de Execução da criptografia: {watch_c.ElapsedMilliseconds} ms");
            Console.WriteLine($"Tempo da Execução: {watch_all.ElapsedMilliseconds} ms");            
            Console.Write("\n\nTexto descriptografado: ");
            Console.WriteLine(Encoding.ASCII.GetString(plainData_2));
            Console.ReadKey();

        }
        static void Greeting()
        {
            Console.WriteLine("| ---------------------------------------------------------- |");
            Console.WriteLine("| Bem vindo ao sistema de criptografia em tempo de execução, |");
            Console.WriteLine("| aqui você pode executar sua criptografia de forma 'unica'. |");
            Console.WriteLine("|  você pode escolher entre a a criptografia RSA ou AES e o  |");
            Console.WriteLine("| tamanho de suas chaves, para no fim obter um arquivo unico |");
            Console.WriteLine("| ---------------------------------------------------------- |\n");

        }

        static ICrypto SelectLib(bool first, in List<string> libs)
        {
            watch_c.Stop();
            Int16 selected;  
            int pos = 0;
            do
            {
                if(libs.Count == 1)
                {
                    selected = 1;
                    break;
                }
                if (first)
                    Console.WriteLine("Deseja começar por qual biblioteca?");
                else
                    Console.WriteLine("Deseja usar por qual biblioteca?");


                pos = 0;
                foreach (var l in libs)
                {
                    pos++;
                    Console.WriteLine($"{pos}){l}");
                }

            }
            while (!(Int16.TryParse(Console.ReadKey().KeyChar.ToString(), out selected) && selected > 0 && selected <= pos));
 
            ICrypto crypto = null;
            switch (libs[selected-1])
            {
                case "RSA":
                    libs.Remove("RSA");
                    crypto= new RSACrypto();
                    break;
                case "AES":
                    libs.Remove("AES");
                    crypto= new AESCrypto();
                    break;                   
            }
            watch_c.Start();
            return crypto;
        }
    }
}