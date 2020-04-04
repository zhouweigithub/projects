// ****************************************
// FileName:RSAUtil.cs
// Description: RSAUtil助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;

namespace Util.Security
{
    /// <summary>
    /// 非对称RSA加密类 可以参考
    /// http://www.cnblogs.com/hhh/archive/2011/06/03/2070692.html
    /// http://blog.csdn.net/zhilunchen/article/details/2943158
    /// 若是私匙加密 则需公钥解密
    /// 反之公钥加密 私匙来解密
    /// 需要BigInteger类来辅助
    /// </summary>
    public static class RSAUtil
    {
        /// <summary>
        /// RSA的容器 可以解密的源字符串长度为 DWKEYSIZE/8-11 
        /// </summary>
        public const Int32 DWKEYSIZE = 1024;

        /// <summary>
        /// RSA加密的密匙结构  公钥和私匙
        /// </summary>
        public struct RSAKey
        {
            public String PublicKey { get; set; }
            public String PrivateKey { get; set; }
        }

        #region 得到RSA的解密的密匙对

        /// <summary>
        /// 得到RSA的解谜的密匙对
        /// </summary>
        /// <returns></returns>
        public static RSAKey GetRASKey()
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            //声明一个指定大小的RSA容器
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(DWKEYSIZE);
            //取得RSA容易里的各种参数
            RSAParameters p = rsaProvider.ExportParameters(true);

            return new RSAKey()
            {
                PublicKey = ComponentKey(p.Exponent, p.Modulus),
                PrivateKey = ComponentKey(p.D, p.Modulus)
            };
        }

        #endregion

        #region 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符

        /// <summary>
        /// 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Boolean CheckSourceValidate(String source)
        {
            return (DWKEYSIZE / 8 - 11) >= source.Length;
        }

        #endregion

        #region 组合解析密匙

        /// <summary>
        /// 组合成密匙字符串
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static String ComponentKey(Byte[] b1, Byte[] b2)
        {
            List<Byte> list = new List<Byte>();
            //在前端加上第一个数组的长度值 这样今后可以根据这个值分别取出来两个数组
            list.Add((Byte)b1.Length);
            list.AddRange(b1);
            list.AddRange(b2);
            Byte[] b = list.ToArray<Byte>();
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 解析密匙
        /// </summary>
        /// <param name="key">密匙</param>
        /// <param name="b1">RSA的相应参数1</param>
        /// <param name="b2">RSA的相应参数2</param>
        private static void ResolveKey(String key, out Byte[] b1, out Byte[] b2)
        {
            //从base64字符串 解析成原来的字节数组
            Byte[] b = Convert.FromBase64String(key);
            //初始化参数的数组长度
            b1 = new Byte[b[0]];
            b2 = new Byte[b.Length - b[0] - 1];
            //将相应位置是值放进相应的数组
            for (Int32 n = 1, i = 0, j = 0; n < b.Length; n++)
            {
                if (n <= b[0])
                {
                    b1[i++] = b[n];
                }
                else
                {
                    b2[j++] = b[n];
                }
            }
        }

        #endregion

        #region 字符串加密解密 公开方法

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="source">源字符串 明文</param>
        /// <param name="key">密匙</param>
        /// <returns>加密遇到错误将会返回原字符串</returns>
        public static String EncryptString(String source, String key)
        {
            String encryptString = String.Empty;
            Byte[] d;
            Byte[] n;
            try
            {
                if (!CheckSourceValidate(source))
                {
                    throw new Exception("source String too long");
                }
                //解析这个密钥
                ResolveKey(key, out d, out n);
                BigInteger biN = new BigInteger(n);
                BigInteger biD = new BigInteger(d);
                encryptString = EncryptString(source, biD, biN);
            }
            catch
            {
                encryptString = source;
            }
            return encryptString;
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="encryptString">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>遇到解密失败将会返回原字符串</returns>
        public static String DecryptString(String encryptString, String key)
        {
            String source = String.Empty;
            Byte[] e;
            Byte[] n;
            try
            {
                //解析这个密钥
                ResolveKey(key, out e, out n);
                BigInteger biE = new BigInteger(e);
                BigInteger biN = new BigInteger(n);
                source = DecryptString(encryptString, biE, biN);
            }
            catch
            {
                source = encryptString;
            }
            return source;
        }

        #endregion

        #region 字符串加密解密 私有  实现加解密的实现方法

        /// <summary>
        /// 用指定的密匙加密 
        /// </summary>
        /// <param name="source">明文</param>
        /// <param name="d">可以是RSACryptoServiceProvider生成的D</param>
        /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>
        /// <returns>返回密文</returns>
        private static String EncryptString(String source, BigInteger d, BigInteger n)
        {
            Int32 len = source.Length;
            Int32 len1 = 0;
            Int32 blockLen = 0;
            if ((len % 128) == 0)
                len1 = len / 128;
            else
                len1 = len / 128 + 1;
            String block = "";
            StringBuilder result = new StringBuilder();
            for (Int32 i = 0; i < len1; i++)
            {
                if (len >= 128)
                    blockLen = 128;
                else
                    blockLen = len;
                block = source.Substring(i * 128, blockLen);
                Byte[] oText = System.Text.Encoding.Default.GetBytes(block);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = biText.modPow(d, n);
                String temp = biEnText.ToHexString();
                result.Append(temp).Append("@");
                len -= blockLen;
            }
            return result.ToString().TrimEnd('@');
        }

        /// <summary>
        /// 用指定的密匙加密 
        /// </summary>
        /// <param name="source">密文</param>
        /// <param name="e">可以是RSACryptoServiceProvider生成的Exponent</param>
        /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>
        /// <returns>返回明文</returns>
        private static String DecryptString(String encryptString, BigInteger e, BigInteger n)
        {
            StringBuilder result = new StringBuilder();
            String[] strarr1 = encryptString.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 i = 0; i < strarr1.Length; i++)
            {
                String block = strarr1[i];
                BigInteger biText = new BigInteger(block, 16);
                BigInteger biEnText = biText.modPow(e, n);
                String temp = System.Text.Encoding.Default.GetString(biEnText.getBytes());
                result.Append(temp);
            }
            return result.ToString();
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 将公钥转换为RSA参数
        /// </summary>
        /// <param name="pemFileConent">公钥pem文件内容</param>
        /// <returns>RSA参数</returns>
        private static RSAParameters ConvertFromPublicKey(String pemFileConent)
        {
            //获得公钥的二进制数据
            Byte[] keyData = Convert.FromBase64String(pemFileConent);

            //判断长度是否足够
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }

            //定义Modulus变量和Exponent变量
            Byte[] pemModulus = new Byte[128];
            Byte[] pemPublicExponent = new Byte[3];

            //进行赋值
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);

            //构造RSA参数并返回
            return new RSAParameters()
            {
                Modulus = pemModulus,
                Exponent = pemPublicExponent
            };
        }

        /// <summary>
        /// 将私钥转换为RSA参数
        /// </summary>
        /// <param name="pemFileConent">私钥pem文件内容</param>
        /// <returns>RSA参数</returns>
        private static RSAParameters ConvertFromPrivateKey(String pemFileConent)
        {
            //获得私钥的二进制数据
            Byte[] keyData = Convert.FromBase64String(pemFileConent);

            //判断长度是否足够
            if (keyData.Length < 609)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }

            //定义变量并进行赋值
            Int32 index = 11;
            Byte[] pemModulus = new Byte[128];
            Array.Copy(keyData, index, pemModulus, 0, 128);

            index += 128;
            index += 2;//141
            Byte[] pemPublicExponent = new Byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);

            index += 3;
            index += 4;//148
            Byte[] pemPrivateExponent = new Byte[128];
            Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

            index += 128;
            index += ((Int32)keyData[index + 1] == 64 ? 2 : 3);//279
            Byte[] pemPrime1 = new Byte[64];
            Array.Copy(keyData, index, pemPrime1, 0, 64);

            index += 64;
            index += ((Int32)keyData[index + 1] == 64 ? 2 : 3);//346
            Byte[] pemPrime2 = new Byte[64];
            Array.Copy(keyData, index, pemPrime2, 0, 64);

            index += 64;
            index += ((Int32)keyData[index + 1] == 64 ? 2 : 3);//412/413
            Byte[] pemExponent1 = new Byte[64];
            Array.Copy(keyData, index, pemExponent1, 0, 64);

            index += 64;
            index += ((Int32)keyData[index + 1] == 64 ? 2 : 3);//479/480
            Byte[] pemExponent2 = new Byte[64];
            Array.Copy(keyData, index, pemExponent2, 0, 64);

            index += 64;
            index += ((Int32)keyData[index + 1] == 64 ? 2 : 3);//545/546
            Byte[] pemCoefficient = new Byte[64];
            Array.Copy(keyData, index, pemCoefficient, 0, 64);

            //构造RSA参数并返回
            return new RSAParameters()
            {
                Modulus = pemModulus,
                Exponent = pemPublicExponent,
                D = pemPrivateExponent,
                P = pemPrime1,
                Q = pemPrime2,
                DP = pemExponent1,
                DQ = pemExponent2,
                InverseQ = pemCoefficient
            };
        }

        /// <summary>
        /// 比较Byte数组
        /// </summary>
        /// <param name="a">A数组</param>
        /// <param name="b">B数组</param>
        /// <returns>结果</returns>
        private static Boolean CompareBytearrays(Byte[] a, Byte[] b)
        {
            //先判断长度
            if (a.Length != b.Length) return false;

            //如果长度相同，则比较每一个字符
            Int32 i = 0;
            foreach (Byte c in a)
            {
                if (c != b[i++]) return false;
            }

            return true;
        }

        /// <summary>
        /// 获取整形的大小
        /// </summary>
        /// <param name="binr">字节读取器</param>
        /// <returns>整形大小</returns>
        private static Int32 GetIntegerSize(BinaryReader binr)
        {
            //定义变量
            Byte bt = 0;
            Byte lowbyte = 0x00;
            Byte highbyte = 0x00;
            Int32 count = 0;

            //读入数据
            bt = binr.ReadByte();

            //expect integer
            if (bt != 0x02) return 0;

            //再读入数据
            bt = binr.ReadByte();

            if (bt == 0x81)
            {
                // data size in next Byte
                count = binr.ReadByte();
            }
            else
            {
                if (bt == 0x82)
                {
                    // data size in next 2 bytes
                    highbyte = binr.ReadByte();
                    lowbyte = binr.ReadByte();

                    Byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    // we already have the data size
                    count = bt;
                }
            }

            //remove high order zeros in data
            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }

            //last ReadByte wasn't a removed zero, so back up a Byte
            binr.BaseStream.Seek(-1, SeekOrigin.Current);

            //返回结果
            return count;
        }

        /// <summary>
        ///  根据私钥构造CSP对象
        /// </summary>
        /// <param name="privkey">二进制的私钥</param>
        /// <returns>CSP对象</returns>
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(Byte[] privkey)
        {
            Byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            //wrap Memory Stream with BinaryReader for easy reading
            BinaryReader binr = new BinaryReader(mem);
            Byte bt = 0;
            UInt16 twobytes = 0;
            Int32 elems = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                //data read as little endian order (actual data order for Sequence is 30 81)
                if (twobytes == 0x8130)
                {
                    //advance 1 Byte
                    binr.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    //advance 2 bytes
                    binr.ReadInt16();
                }
                else
                {
                    return null;
                }

                twobytes = binr.ReadUInt16();
                //version number
                if (twobytes != 0x0102) return null;

                bt = binr.ReadByte();
                if (bt != 0x00) return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSAParameters rsaParams = new RSAParameters()
                {
                    Modulus = MODULUS,
                    Exponent = E,
                    D = D,
                    P = P,
                    Q = Q,
                    DP = DP,
                    DQ = DQ,
                    InverseQ = IQ
                };

                //定义CSP对象
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.ImportParameters(rsaParams);

                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        /// <summary>
        /// 根据私钥构造CSP对象
        /// </summary>
        /// <param name="pkcs8">二进制的私钥</param>
        /// <returns>CSP对象</returns>
        private static RSACryptoServiceProvider DecodePrivateKeyInfo(Byte[] pkcs8)
        {
            Byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            Byte[] seq = new Byte[15];

            //将私钥数据读入内存
            MemoryStream mem = new MemoryStream(pkcs8);
            Int32 lenstream = (Int32)mem.Length;
            //wrap Memory Stream with BinaryReader for easy reading
            BinaryReader binr = new BinaryReader(mem);
            Byte bt = 0;
            UInt16 twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                //data read as little endian order (actual data order for Sequence is 30 81)
                if (twobytes == 0x8130)
                {
                    //advance 1 Byte
                    binr.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    //advance 2 bytes
                    binr.ReadInt16();
                }
                else
                {
                    return null;
                }

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                //read the Sequence OID
                seq = binr.ReadBytes(15);
                //make sure Sequence for OID is correct
                if (!CompareBytearrays(seq, SeqOID)) return null;

                bt = binr.ReadByte();
                //expect an Octet String 
                if (bt != 0x04) return null;

                //read next Byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the Byte count
                bt = binr.ReadByte();
                if (bt == 0x81)
                {
                    binr.ReadByte();
                }
                else
                {
                    if (bt == 0x82)
                    {
                        binr.ReadUInt16();
                    }
                }

                //------ at this stage, the remaining sequence should be the RSA private key
                Byte[] rsaprivkey = binr.ReadBytes((Int32)(lenstream - mem.Position));
                return DecodeRSAPrivateKey(rsaprivkey);
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        /// <summary>
        /// 根据私钥构造CSP对象
        /// </summary>
        /// <param name="pemFileConent">私钥字符串</param>
        /// <returns>CSP对象</returns>
        private static RSACryptoServiceProvider DecodePemPrivateKey(String pemFileConent)
        {
            //将私钥数据进行解码
            Byte[] pkcs8privatekey = Convert.FromBase64String(pemFileConent);

            //判断是否为空，如果不为空则构造对象
            if (pkcs8privatekey != null)
            {
                return DecodePrivateKeyInfo(pkcs8privatekey);
            }

            return null;
        }

        /// <summary>
        /// 使用私钥进行解密
        /// </summary>
        /// <param name="data">加密的字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>解密后的字符串</returns>
        private static String Decrypt(Byte[] data, String privateKey)
        {
            //根据私钥得到CSP对象
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);

            //构造SHA1对象
            SHA1 sh = new SHA1CryptoServiceProvider();

            //得到解密后的二进制数据
            Byte[] source = rsa.Decrypt(data, false);

            //将解密后的数据转化为String
            Char[] asciiChars = new Char[Encoding.UTF8.GetCharCount(source, 0, source.Length)];
            Encoding.UTF8.GetChars(source, 0, source.Length, asciiChars, 0);

            return new String(asciiChars);
        }

        #endregion

        #region 供其它类调用的方法

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content">待签名字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名后字符串</returns>
        public static String SignWithSHA1(String content, String privateKey)
        {
            //获得待签名字符串的二进制
            Byte[] data = Encoding.UTF8.GetBytes(content);

            //根据私钥得到CSP
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);

            //构造SHA1对象
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            //得到签名后的数据的二进制
            Byte[] signData = rsa.SignData(data, sha1);

            //对签名的数据进行Base64编码，并返回字符串
            return Convert.ToBase64String(signData);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content">待签名字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名后字符串</returns>
        public static String SignWithMD5(String content, String privateKey)
        {
            //获得待签名字符串的二进制
            Byte[] data = Encoding.UTF8.GetBytes(content);

            //根据私钥得到CSP
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);

            //构造MD5对象
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //得到签名后的数据的二进制
            Byte[] signData = rsa.SignData(data, md5);

            //对签名的数据进行Base64编码，并返回字符串
            return Convert.ToBase64String(signData);
        }

        /// <summary>
        /// 验证签名是否正确
        /// </summary>
        /// <param name="verifyString">待验证字符串</param>
        /// <param name="signString">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public static Boolean VerifyWithSHA1(String verifyString, String signString, String publicKey)
        {
            //获得待验证字符串的二进制
            Byte[] verifyData = Encoding.UTF8.GetBytes(verifyString);

            //获得当前签名的二进制
            Byte[] signData = Convert.FromBase64String(signString);

            //构造RSA服务提供者对象，并导入参数
            RSACryptoServiceProvider serviceProvider = new RSACryptoServiceProvider();
            serviceProvider.ImportParameters(ConvertFromPublicKey(publicKey));

            //构造SHA1对象
            SHA1 sh = new SHA1CryptoServiceProvider();

            //返回验证结果
            return serviceProvider.VerifyData(verifyData, sh, signData);
        }

        /// <summary>
        /// 验证签名是否正确
        /// </summary>
        /// <param name="verifyString">待验证字符串</param>
        /// <param name="signString">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public static Boolean VerifyWithMD5(String verifyString, String signString, String publicKey)
        {
            //获得待验证字符串的二进制
            Byte[] verifyData = Encoding.UTF8.GetBytes(verifyString);

            //获得当前签名的二进制
            Byte[] signData = Convert.FromBase64String(signString);

            //构造RSA服务提供者对象，并导入参数
            RSACryptoServiceProvider serviceProvider = new RSACryptoServiceProvider();
            serviceProvider.ImportParameters(ConvertFromPublicKey(publicKey));

            //构造SHA1对象=
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //返回验证结果
            return serviceProvider.VerifyData(verifyData, md5, signData);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="resData">加密字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns>明文</returns>
        public static String DecryptData(String resData, String privateKey)
        {
            //获得用于解密的字符串的二进制数据
            Byte[] dataToDecrypt = Convert.FromBase64String(resData);

            String result = "";
            //遍历数组，进行解密
            for (Int32 j = 0; j < dataToDecrypt.Length / 128; j++)
            {
                Byte[] buf = new Byte[128];
                for (Int32 i = 0; i < 128; i++)
                {
                    buf[i] = dataToDecrypt[i + 128 * j];
                }
                result += Decrypt(buf, privateKey);
            }
            return result;
        }

        /// <summary>
        /// 使用公钥进行解密
        /// </summary>
        /// <param name="encryptData">加密数据</param>
        /// <param name="publicKeyFilePath">公钥文件路径</param>
        /// <param name="isDirect">是否直接解密，PP属于这种情况</param>
        /// <returns>解密数据</returns>
        public static String Decrypt(String encryptData, String publicKeyFilePath, Boolean isDirect = false)
        {
            StreamReader pubPemFileStream = File.OpenText(publicKeyFilePath);
            PemReader pubPemReader = new PemReader(pubPemFileStream);
            ICipherParameters pubKeyParam = (AsymmetricKeyParameter)pubPemReader.ReadObject();

            IAsymmetricBlockCipher eng = new Pkcs1Encoding(new RsaEngine());
            eng.Init(false, pubKeyParam);

            //获得待解密字符串的Byte[]格式
            Byte[] notifyData = Convert.FromBase64String(encryptData);

            //如果直接解密，则直接解密并返回
            if(isDirect)
            {
                return Encoding.UTF8.GetString(eng.ProcessBlock(notifyData, 0, notifyData.Length));
            }

            Int32 index = 0;
            String decryptedJsonStr = "";
            while (index < notifyData.Length)
            {
                Int32 len = notifyData.Length - index;
                if (len > 128)
                {
                    len = 128;
                }
                Byte[] decryptedTmpData = new Byte[len];
                Array.Copy(notifyData, index, decryptedTmpData, 0, len);
                index += len;
                decryptedJsonStr += Encoding.UTF8.GetString(eng.ProcessBlock(decryptedTmpData, 0, len));
            }

            return decryptedJsonStr;
        }

        #endregion
    }
}