//***********************************************************************************
//文件名称：ReflectionUtil.cs
//功能描述：使用反射的方式，动态调用类型方法，支持实例方法与静态方法的调用
//数据表：Nothing
//作者：byron
//日期：2014/3/12 14:12:06
//修改记录：
//***********************************************************************************

using System;
using System.Linq;
using System.Reflection;

namespace Util.Reflection
{
    /// <summary>
    /// 反射助手类
    /// </summary>
    public static class ReflectionUtil
    {
        #region 公共方法

        /// <summary>
        /// 创建ReflectionInvoke.ReflectionClass对象的实例
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">要反射的类名称</param>
        /// <returns>创建的ReflectionInvoke.ReflectionClass对象</returns>
        public static ReflectionClass CreateInstance(String assemblyName, String className)
        {
            return CreateInstance(assemblyName, className, null);
        }

        /// <summary>
        /// 创建ReflectionInvoke.ReflectionClass对象
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">要反射的类名称</param>
        /// <param name="param">调用反射类的构造函数所需要的参数</param>
        /// <returns>创建的ReflectionInvoke.ReflectionClass对象</returns>
        public static ReflectionClass CreateInstance(String assemblyName, String className, params Object[] param)
        {
            //获取类型对象
            Type t = GetClassType(assemblyName, className);
            Object classObject = Activator.CreateInstance(t, param);

            return new ReflectionClass(classObject);
        }

        /// <summary>
        /// 调用类中的静态方法
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">静态方法所在的类</param>
        /// <param name="methodName">方法名称</param>
        /// <returns>调用方法所返回的值</returns>
        public static Object CallStaticMethod(String assemblyName, String className, String methodName)
        {
            return CallStaticMethod(assemblyName, className, methodName, Type.EmptyTypes, null);
        }

        /// <summary>
        /// 调用类中的静态方法
        ///     注：
        ///         param实参中中不能包含null，如要传递null，请使用CallStaticMethod(String ,String ,String ,Type[] ,param Object[] )重载方法
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">静态方法所在的类</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="param">方法所需要的参数</param>
        /// <returns>调用方法所返回的值</returns>
        public static Object CallStaticMethod(String assemblyName, String className, String methodName, params Object[] param)
        {
            return CallStaticMethod(assemblyName, className, methodName, GetParamTypes(param), param);
        }

        /// <summary>
        /// 调用类中的静态方法
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">静态方法所在的类</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="types">参数类型列表</param>
        /// <param name="param">方法所需要的参数</param>
        /// <returns>调用方法所返回的值</returns>
        public static Object CallStaticMethod(String assemblyName, String className, String methodName, Type[] types, params Object[] param)
        {
            MethodInfo method = GetMethod(assemblyName, className, methodName, types);
            if (method == null)
            {
                throw new AmbiguousMatchException(String.Format("未能在类{0}中发现{1}方法。", className, methodName));
            }

            return method.Invoke(null, param);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 检查参数的存在性
        /// </summary>
        /// <param name="paramNames">参数名称列表</param>
        /// <param name="paramNameItem">参数名称</param>
        /// <returns>是否存在</returns>
        private static String CheckParamExists(String[] paramNames, String paramNameItem)
        {
            foreach (String item in paramNames)
            {
                if (String.Equals(item, paramNameItem, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据参数获取参数对应的类型
        /// </summary>
        /// <param name="param">参数列表</param>
        /// <returns>与参数列表对应的参数类型</returns>
        internal static Type[] GetParamTypes(params Object[] param)
        {
            Type[] resultTypes = Type.EmptyTypes;
            if (param != null && param.Length > 0)
            {
                resultTypes = param.Select(s => s.GetType()).ToArray();
            }

            return resultTypes;
        }

        /// <summary>
        /// 返回指定类型的类型对象
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">要获取对象类型的类名称</param>
        /// <returns>该类所对应的类型</returns>
        private static Type GetClassType(String assemblyName, String className)
        {
            String key = String.Join("_", assemblyName, className);
            Type t = ReflectionCache.GetType(key);
            if (t == null)
            {
                //如果缓存中不存在，则使用反射获取对象，并且存入缓存中
                t = AssemblyType(assemblyName, className);
                ReflectionCache.SetType(key, t);
            }

            return t;
        }

        /// <summary>
        /// 返回指定的方法类型对象
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">要获取对象类型的类名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="types">方法所需要的参数</param>
        /// <returns>该类所对应的类型</returns>
        private static MethodInfo GetMethod(String assemblyName, String className, String methodName, Type[] types)
        {
            String key = String.Join("_", assemblyName, className, methodName, String.Join<Type>("_", types));
            MethodInfo method = ReflectionCache.GetMehtod(key);
            if (method == null)
            {
                //如果缓存中不存在方法，则使用反射获取方法，并且存入缓存中
                Type t = GetClassType(assemblyName, className);
                method = t.GetMethod(methodName, types);
                ReflectionCache.SetMethod(key, method);
            }

            return method;
        }

        /// <summary>
        /// 反射指定类型
        /// </summary>
        /// <param name="assemblyName">要反射的程序集名称</param>
        /// <param name="className">要反射的类名称</param>
        /// <returns>该类所对应的类型</returns>
        private static Type AssemblyType(String assemblyName, String className)
        {
            Assembly ass = Assembly.Load(assemblyName);

            return ass.GetType(className);
        }

        #endregion
    }
}