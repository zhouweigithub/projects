// ****************************************************
// FileName:CommonData.cs
// Description:
// Tables:
// Author:ZhouWei
// Create Date:2020-11-11
// Revision History:
// ****************************************************
using System.Text;

namespace FormatCode
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