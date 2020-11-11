using System.Text;

namespace test
{
    public static class CommonData
    {
        /// <summary>
        /// UTF8文件编码
        /// </summary>
        public static readonly Encoding EncodingUTF8 = Encoding.UTF8;

        /// <summary>
        /// GB2312文件编码
        /// </summary>
        public static readonly Encoding EncodingGB2312 = Encoding.GetEncoding("GB2312");
    }
}
