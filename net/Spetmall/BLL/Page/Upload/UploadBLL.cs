using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page.Upload
{
    public class UploadBLL
    {

        /// <summary> 
        /// 缩放图片 
        /// </summary> 
        /// <param name="image">Image 对象</param> 
        /// <param name="width">图片新的宽度</param> 
        /// <param name="height">图片新高度</param> 
        /// <param name="scale">是否按比例缩放图片</param> 
        /// <returns>Image 对象</returns> 
        public static Image Resize(Image image, int width, int height, bool scaleable)
        {
            // 定义图片的新尺寸 
            int iWidth, iHeight;

            // 如果是按比例缩放图片(即scaleable = true)，生成的图片的尺寸以不超过指定尺寸为准，否则以绝对尺寸为准 
            if (scaleable)
            {
                if (image.Width > image.Height)
                {
                    iWidth = width;
                    iHeight = image.Height * iWidth / image.Width;
                }
                else
                {
                    iHeight = height;
                    iWidth = image.Width * iHeight / image.Height;
                }
            }
            else
            {
                iWidth = width;
                iHeight = height;
            }

            Rectangle r = new Rectangle(0, 0, iWidth, iHeight);
            Image img = new Bitmap(iWidth, iHeight);
            using (Graphics g = Graphics.FromImage(img))
            {
                // 定义缩放图片为高质量 
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed; 
                g.DrawImage(image, r);
            }
            return img;
        }

        public static void SaveMap(Image image, string filePath, bool compressible)
        {
            if (compressible)
            {
                ImageCodecInfo ici = ImageCodecInfo.GetImageEncoders().Where(p => p.MimeType.ToLower() == "image/jpeg").FirstOrDefault();
                EncoderParameters ep = new EncoderParameters();
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt32(40)); //这里最后一个参数,需要注意,即使给的参数是整数,也必须要转成Int32,否则会被默认为byte类型!
                image.Save(filePath, ici, ep);
            }
            else
            {
                image.Save(filePath);
            }
        }
    }
}
