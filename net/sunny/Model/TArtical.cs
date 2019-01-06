using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sunny.Model
{
    public class MArticalBase
    {
        [TableField]
        public int Id { get; set; }
        [TableField]
        public string Title { get; set; }
        [TableField]
        public int Year { get; set; }
        [TableField]
        public string Area { get; set; }

    }

    public class MArtical : MArticalBase
    {
        [TableField]
        public string Content { get; set; }
        [TableField]
        public string Domain { get; set; }
        [TableField]
        public DateTime CrTime { get; set; }
        [TableField]
        public string Type { get; set; }
    }
}
