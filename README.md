# Sistema de criptografa em C# 
 
**Criador**: Gabriel Felipe Krabbe 

Esse sistema visa a possibilidade de criptografar  um texto plano de forma fácil e rápida.

É possivel de escolher entre 2 formas de criptografia:
 - RSA
 - AES

Alem da da forma de criptografia cripgrafia, tambem é possivel escolher o tamanho da chave.

## Construção

Sua construção é relativamente simples, bastando somente importar a classe `System.Security.Cryptography` e escolher entre os inumeros tipos de cripgrafia.

### criptografia RSA

1. Para providenciar um processo de criptografa RSA basta iniciar a classe `RSACryptoServiceProvider`.

2. Para definir um tamanho de chave basta alterar o `RSA.KeySize`.

3. Para encriptar basta usar a função `RSA.Encrypt`, passando o valor em byte[]e na sequencia passar se deseja usar o função `OAEP`.

4. Para descriptografar basta usar a função `RSA.Decrypt`, passando o valor em byte[] e na sequencia passar se deseja usar o função `OAEP`.

EX:
```C#
using System.Security.Cryptography;

public class RSACrypto                    
    {
        public readonly RSACryptoServiceProvider RSA;
        public RSACrypto()
        {
            RSA = new RSACryptoServiceProvider();
            RSA.KeySize = 255;
        }

        public byte[] Encrypt(byte[] plainData)
        {
            return RSA.Encrypt(plainData, false);
        }

        public byte[] Decrypt(byte[] cipherData)
        {
            return RSA.Decrypt(cipherData, false);;
        }        
    }
```

### criptografia AES

1. Para providenciar um processo de criptografa AES basta iniciar a classe `RijndaelManaged`.

2. Para definir um tamanho de chave basta alterar o `myRijndael.KeySize`.

3. Para encriptar é necessário que 5 passos:
    1. Primeiro é necessário criar o tranformador   , para isso devemos chamar a função `myRijndael.CreateEncryptor()`
    
    2. Na sequencia devemos criar um class de MemoryStream, ela servirar para armazenar a transformação.
    
    3. Em seguida devemos criar um class de CryptoStream, ela servirar criptografar em tempo real o dado, passando como parametro o MemoryStream, o transformador e o sinalizador de modo de escrita, no caso `CryptoStreamMode.Write`.

    4. Devemos escrever o dado plano (em forma de byte[]) para dentro do CryptoStream, usando ` cryptoStream.Write(data, 0, data.Length);` e na sequencia `cryptoStream.FlushFinalBlock()`
    
    5. Por ultimos pegamos os dado criptografado usando `memoryStream.ToArray();`

4. A Descriptografar é muito semelhante ao encriptação, é necessário que 5 passos:
    1. Primeiro é necessário criar o tranformadorm, para isso devemos chamar a função `myRijndael.CreateDecryptor()`.

    2. Na sequencia devemos criar um class de MemoryStream, ela servirar para armazenar a transformação.
    
    3. Em seguida devemos criar um class de CryptoStream, ela servirar descriptografar em tempo real o dado, passando como parametro o MemoryStream, o transformador e o sinalizador de modo de escrita, no caso `CryptoStreamMode.Write`.

    4. Devemos escrever o dado plano (em forma de byte[]) para dentro do CryptoStream, usando ` cryptoStream.Write(data, 0, data.Length);` e na sequencia `cryptoStream.FlushFinalBlock()`
    
    5. Por ultimos pegamos os dado descriptografado usando `memoryStream.ToArray();`

EX:

```C#

using System.Security.Cryptography;

 public class AESCrypto : ICrypto
    {
        readonly RijndaelManaged myRijndael;
        public AESCrypto()
        {
            myRijndael = new RijndaelManaged();
            myRijndael.KeySize =255;

        }
        public byte[] Encrypt(byte[] plainData)
        {
            var cryptoTransform = myRijndael.CreateEncryptor();
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

        public byte[] Decrypt(byte[] cipherData)
        {
            var cryptoTransform = myRijndael.CreateDecryptor();
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

```