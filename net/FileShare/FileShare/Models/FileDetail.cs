using System;

namespace FileShare.Models
{
    public class FileDetail
    {
        /// <summary>
        /// 文件名或目录路径
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public Int64 Size { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 是否为目录
        /// </summary>
        public Boolean IsFolder { get; set; }
        /// <summary>
        /// 自己是否为创建人
        /// </summary>
        public Boolean IsCreater { get; set; }

        public String Date => LastModifyTime.ToString("yyyy-MM-dd");

        public String Time => LastModifyTime.ToString("HH:mm");
        public String SizeString
        {
            get
            {
                Double ksize = Math.Ceiling((Double)Size / 1000);
                return ksize.ToString("N0") + " K";
            }
        }
    }
}