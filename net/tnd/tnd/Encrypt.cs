using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace tnd
{
    /// <summary>
    /// 加密解密工具类
    /// 作者博客：https://www.cnblogs.com/tuyile006/
    /// </summary>
    public class Encrypt
    {
        #region MD5
        /// <summary>
        /// MD5哈希加密
        /// </summary>
        /// <param name="scr">原始string数据</param>
        /// <returns>加密后的数据</returns>
        public static string MD5(string scr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] palindata = Encoding.Default.GetBytes(scr);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(palindata);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }
        #endregion

        #region SHA1
        /// <summary>
        /// SHA1哈希加密
        /// </summary>
        /// <param name="scr">原始string数据</param>
        /// <returns>加密后的数据</returns>
        public static string SHA1(string scr)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] palindata = Encoding.Default.GetBytes(scr);//将要加密的字符串转换为字节数组
            byte[] encryptdata = sha1.ComputeHash(palindata);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }
        #endregion

        #region RSA
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="scr">原始string数据</param>
        /// <returns></returns>
        public static string RSA(string scr)
        {
            CspParameters csp = new CspParameters
            {
                //在Web中配置参见https://docs.microsoft.com/zh-cn/previous-versions/aspnet/yxw286t2%28v%3dvs.100%29
                KeyContainerName = "tuyile006.cnblogs.com"//密匙容器的名称，保持加密解密一致才能解密成功
            }; //密钥容器知识参见https://docs.microsoft.com/zh-cn/dotnet/standard/security/how-to-store-asymmetric-keys-in-a-key-container
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp))
            {
                byte[] plaindata = Encoding.Default.GetBytes(scr);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="scr">密文</param>
        /// <returns></returns>
        public static string RSADecrypt(string scr)
        {
            try
            {
                CspParameters csp = new CspParameters
                {
                    KeyContainerName = "tuyile006.cnblogs.com"//密匙容器的名称，保持加密解密一致才能解密成功
                };
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp))
                {
                    byte[] bytes = Convert.FromBase64String(scr); //加密时用了Base64，则解密时对应的也要用Base64解码
                    byte[] DecryptBytes = rsa.Decrypt(bytes, false);
                    return Encoding.Default.GetString(DecryptBytes);
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 返回RSA公匙
        /// </summary>
        /// <returns></returns>
        public static string GetRSAPublicKey()
        {
            CspParameters csp = new CspParameters
            {
                KeyContainerName = "tuyile006.cnblogs.com"//密匙容器的名称，保持加密解密一致才能解密成功
            };
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp))
            {
                return rsa.ToXmlString(false);
            }
        }

        #endregion

        #region DES
        private const string DesIV_64 = "xiaoy><@";//定义默认加密密钥 8个字节 
        /// <summary>
        /// 按指定键值进行DES加密
        /// </summary>
        /// <param name="strContent">要加密字符</param>
        /// <param name="strKey">自定义键值 ASCII编码  必须大于或等于8个字符</param>
        /// <returns></returns>
        public static string DES(string strContent, string strKey)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            if (strKey.Length > 8) strKey = strKey.Substring(0, 8);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            byte[] byKey = Encoding.ASCII.GetBytes(strKey);
            byte[] byIV = Encoding.ASCII.GetBytes(DesIV_64);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cst))
                    {
                        sw.Write(strContent);
                        sw.Flush();
                        cst.FlushFinalBlock();
                        sw.Flush();
                        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 按指定键值进行DES解密
        /// </summary>
        /// <param name="strContent">要解密字符</param>
        /// <param name="strKey">加密时使用的键值 ASCII编码 必须大于或等于8个字符</param>
        /// <returns></returns>
        public static string DESDecrypt(string strContent, string strKey)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            if (strKey.Length > 8) strKey = strKey.Substring(0, 8);
            byte[] byKey = Encoding.ASCII.GetBytes(strKey);
            byte[] byIV = Encoding.ASCII.GetBytes(DesIV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(strContent);
                using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
                {
                    using (MemoryStream ms = new MemoryStream(byEnc))
                    {
                        using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read))
                        {
                            StreamReader sr = new StreamReader(cst);
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

        }
        #endregion

        #region AES
        private const string AesIV_128 = "xiaoy设计.";//定义默认加密密钥 16个字节 Unicode编码为8个英文或汉字
        /// <summary>
        /// 按指定键值进行AES加密
        /// </summary>
        /// <param name="plainText">要解密字符</param>
        /// <param name="strKey">加密时使用的键值 Unicode编码 必须大于或等于8个英文或汉字</param>
        /// <returns></returns>
        public static string AES(string strContent, string strKey)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            if (strKey.Length > 8) strKey = strKey.Substring(0, 8);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.Unicode.GetBytes(strKey);
                aesAlg.IV = Encoding.Unicode.GetBytes(AesIV_128);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(strContent);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray()); //返回Base64密文方便传输
                    }
                }
            }
        }
        /// <summary>
        /// 按指定键值进行AES解密
        /// </summary>
        /// <param name="strContent">要解密字符</param>
        /// <param name="strKey">加密时使用的键值 Unicode编码 必须大于或等于8个英文或汉字</param>
        /// <returns></returns>
        public static string AESDecrypt(string strContent, string strKey)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            if (strKey.Length > 8) strKey = strKey.Substring(0, 8);
            //与加密时Base64对应
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(strContent);
                //解密
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = Encoding.Unicode.GetBytes(strKey);
                    aesAlg.IV = Encoding.Unicode.GetBytes(AesIV_128);

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(byEnc))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region ECC

        /// <summary>
        /// 利用ecc生成key
        /// 假设从A-->B进行信息发送
        /// </summary>
        /// <param name="AKeyName">A的公钥名称 自身</param>
        /// <param name="BKey">B的公钥</param>
        /// <returns> 生成A端用于交互信息的密钥，可以用于AES加密的密钥</returns>
        public static string ECC_EncodeKey(string AKeyName, string BKey)
        {
            byte[] BKeybyte = Convert.FromBase64String(BKey);
            using (ECDiffieHellmanCng AClient = new ECDiffieHellmanCng(CngKey.Open(AKeyName)))
            //using (ECDiffieHellmanCng AClient = new ECDiffieHellmanCng())
            {
                AClient.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                AClient.HashAlgorithm = CngAlgorithm.Sha256;

                byte[] MsgKey = AClient.DeriveKeyMaterial(CngKey.Import(BKeybyte, CngKeyBlobFormat.EccPublicBlob));
                return Convert.ToBase64String(MsgKey);
            }
        }

        /// <summary>
        /// 获取自身的公钥
        /// </summary>
        /// <returns>Base64编码的字符串，接收端需要Base64解码再使用</returns>
        public static string ECC_GetMyPublicKey(string keyName)
        {
            if (!CngKey.Exists(keyName))
            {
                using (ECDiffieHellmanCng MyECC = new ECDiffieHellmanCng(CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, keyName)))
                {
                    MyECC.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                    MyECC.HashAlgorithm = CngAlgorithm.Sha256;
                    byte[] Keybyte = MyECC.PublicKey.ToByteArray();
                    return Convert.ToBase64String(Keybyte);

                }
            }
            else
            {
                using (ECDiffieHellmanCng MyECC = new ECDiffieHellmanCng(CngKey.Open(keyName)))
                {
                    byte[] Keybyte = MyECC.PublicKey.ToByteArray();
                    return Convert.ToBase64String(Keybyte);
                }
            }
        }
        #endregion
    }
}