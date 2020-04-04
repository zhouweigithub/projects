using System;
using System.IO;
using System.Text;

namespace tnd
{
    public class BLL
    {
        private const string aesKey = "501f0205-4f71-44d2-8243-e44a96516593";
        private static readonly Encoding encoding = Encoding.Default;


        public static string EnCode(string inPath)
        {
            string content = GetFileContent(inPath);
            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("source file is not exist or empty");

            string encodeText = Encrypt.AES(content, aesKey);
            return encodeText;
        }

        public static string DeCode(string inPath)
        {
            string content = GetFileContent(inPath);
            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("source file not exist");

            string decodeText = Encrypt.AESDecrypt(content, aesKey);
            return decodeText;
        }

        public static void EnCode(string inPath, string outPath)
        {
            string encodeText = EnCode(inPath);
            WriteFileContent(outPath, encodeText);
        }

        public static void DeCode(string inPath, string outPath)
        {
            string encodeText = DeCode(inPath);
            WriteFileContent(outPath, encodeText);
        }

        private static string GetFileContent(string path)
        {
            if (!File.Exists(path))
                return string.Empty;

            return File.ReadAllText(path, encoding);
        }

        private static void WriteFileContent(string path, string content)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("save path is not exist");

            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(path, content, encoding);
        }
    }
}
