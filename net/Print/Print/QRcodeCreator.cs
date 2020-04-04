using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Print
{
    class QRcodeCreator
    {
        ///// <summary>
        ///// 根据链接获取二维码
        ///// </summary>
        ///// <param name="url">链接</param>
        ///// <returns>返回二维码图片</returns>
        //public static Image GetQRCodeBmp(string url)
        //{
        //    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        //    qrCodeEncoder.QRCodeScale = 4;
        //    qrCodeEncoder.QRCodeVersion = 0;
        //    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        //    return qrCodeEncoder.Encode(url);
        //}
    }
}
