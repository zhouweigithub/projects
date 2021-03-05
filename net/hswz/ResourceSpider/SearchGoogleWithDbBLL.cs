using System;
using System.Collections.Generic;
using System.Linq;
using Hswz.DAL;
using Hswz.Model.Urls;

namespace ResourceSpider
{
    public class SearchGoogleWithDbBLL : SearchGoogleBase
    {
        protected override List<String> GetSavedUrls()
        {
            var datas = DBData.GetInstance(DBTable.url).GetList<urls>();
            if (datas != null && datas.Count > 0)
            {
                return datas.Select(a => a.url).ToList();
            }
            else
            {
                return new List<String>();
            }
        }

        protected override Boolean SaveData(String url)
        {
            return DBData.GetInstance(DBTable.url).Add(new urls()
            {
                url = url,
                status = 0,
            }) > 0;
        }
    }
}
