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
    public class DiscountBLL
    {
        public static discount_edit GetEditInfo(int id)
        {
            try
            {
                discount_edit entity = discountDAL.GetInstance().GetEntityByKey<discount_edit>(id);

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
                Util.Log.LogUtil.Write("GetEditInfo 获取限时折扣活动数据出错\r\n" + e, Util.Log.LogType.Error);
            }

            return null;
        }
    }
}
