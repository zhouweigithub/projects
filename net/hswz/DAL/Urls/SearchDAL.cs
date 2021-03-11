using System;

namespace Hswz.DAL.Urls
{
    public class SearchDAL
    {


        //public List<ResourceDetail> GetList(String key, int page, int pageSize)
        //{
        //    string where = GetWhere(key);
        //    DBData.GetInstance
        //}

        private String GetWhere(String key)
        {
            String result = String.Empty;
            if (!String.IsNullOrWhiteSpace(key))
            {
                result += " and title like ?";
            }

            return result;
        }
    }
}
