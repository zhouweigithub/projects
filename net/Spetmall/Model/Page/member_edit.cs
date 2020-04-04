using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    public class member_edit : member
    {
        public string pets { get; set; }
        /// <summary>
        /// 喜爱的宠物id
        /// </summary>
        public List<string> petList
        {
            get
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrWhiteSpace(pets))
                {
                    var tmpArray = pets.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    list.AddRange(tmpArray);
                }
                return list;
            }
        }
    }
}
