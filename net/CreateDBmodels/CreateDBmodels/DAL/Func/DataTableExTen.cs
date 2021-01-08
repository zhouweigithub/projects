//***********************************************************************************
//文件名称：DataTableExtentsion.cs
//功能描述：DataTable转化泛型实体
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CreateDBmodels.DAL
{
    public static class DataTableExtentsion
    {

        /// <summary>
        /// DataTable转化泛型实体
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="dt"></param>
        /// <returns>TResult的泛型</returns>
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            //创建一个属性的列表  
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();

            //获取TResult的类型实例  反射的入口  
            Type t = typeof(TResult);

            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表   
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) { propertyInfoList.Add(p); } });

            //创建返回的集合  
            List<TResult> resultList = new List<TResult>();
            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例  
                TResult result = new TResult();

                //找到对应的数据  并赋值  
                propertyInfoList.ForEach(p => { if (row[p.Name] != DBNull.Value) { p.SetValue(result, Convert.ChangeType(row[p.Name], p.PropertyType), null); } });

                //放入到返回的集合中.  
                resultList.Add(result);
            }

            return resultList;
        }

    }
}
