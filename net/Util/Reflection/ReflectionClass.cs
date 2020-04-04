//***********************************************************************************
//文件名称：ReflectionClass.cs
//功能描述：使用反射的方式，用于表示通过反射创建的实例对象
//数据表：Nothing
//作者：byron
//日期：2014/3/12 14:12:06
//修改记录：
//***********************************************************************************

using System;
using System.Reflection;

namespace Util.Reflection
{
    /// <summary>
    /// 反射构造的实例类对象，可以通过该类调用实例方法
    /// </summary>
    public class ReflectionClass
    {
        #region 字段

        //类对象
        private Object classObject = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="obj">反射出的类类型对象</param>
        /// <param name="reflectionInvoke">与ReflectionClass相关联的ReflectionInvoke对象</param>
        internal ReflectionClass(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("参数不能为null。", "obj");
            }                

            this.classObject = obj;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 调用反射类型的实例方法
        /// </summary>
        /// <param name="methodName">要调用的方法名称</param>
        /// <returns>调用方法的返回值</returns>
        public Object CallInstanceMethod(String methodName)
        {
            return CallInstanceMethod(methodName, Type.EmptyTypes, null);
        }

        /// <summary>
        /// 调用反射类型的实例方法
        ///     注：
        ///         方法参数不能包含null值，如果要传递null，请使用CallInstanceMethod(String , Type[] , params Object[] )重载
        /// </summary>
        /// <param name="methodName">要调用的方法名称</param>
        /// <param name="param">与方法匹配的参数</param>
        /// <returns>调用方法的返回值</returns>
        public Object CallInstanceMethod(String methodName, params Object[] param)
        {
            return CallInstanceMethod(methodName, ReflectionUtil.GetParamTypes(param), param);
        }

        /// <summary>
        /// 调用反射类型的实例方法
        /// </summary>
        /// <param name="methodName">要调用的方法名称</param>
        /// <param name="paramTypes">参数对应的类型列表</param>
        /// <param name="param">与方法匹配的参数列表</param>
        /// <returns>调用方法的返回值</returns>
        public Object CallInstanceMethod(String methodName, Type[] paramTypes, params Object[] param)
        {
            MethodInfo method = classObject.GetType().GetMethod(methodName, paramTypes);
            if (method == null)
            {
                throw new AmbiguousMatchException(String.Format("未能在类{0}中发现{1}方法。", classObject.GetType().FullName, methodName));
            }

            return method.Invoke(classObject, param);
        }

        #endregion
    }
}