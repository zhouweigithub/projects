using Spetmall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spetmall.DAL;
using Spetmall.Model.Page;

namespace Spetmall.BLL.Page
{
    public class FullSendBLL
    {
        public static fullsend_edit GetEditInfo(int id)
        {
            try
            {
                fullsend_edit entity = fullsendDAL.GetInstance().GetEntityByKey<fullsend_edit>(id);

                if (entity == null)
                    return null;

                IList<salerule> rules = saleRuleDAL.GetInstance().GetList<salerule>($"saleid={id}");
                IList<saleproductObject> products = saleProductDAL.GetEditDatas(id);

                if (rules != null)
                    entity.rules = rules;

                if (products != null)
                    entity.products = products;

                return entity;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("GetEditInfo 获取满就送活动数据出错\r\n" + e, Util.Log.LogType.Error);
            }

            return null;
        }
    }
}
