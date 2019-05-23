// ****************************************
// FileName:ServiceInstallBLL.cs
// Description: Windows服务安装帮助类
// Tables:
// Author:Gavin
// Create Date:2015/1/15 16:14:54
// Revision History:
// ****************************************

using System;
using System.Linq;
using System.Collections;
using System.ServiceProcess;
using System.Configuration.Install;

namespace Util
{
    /// <summary>
    /// Windows服务安装帮助类
    /// </summary>
    public static class ServiceInstallUtil
    {
        #region 检查服务是否存在

        /// <summary>
        /// 检查服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>服务是否存在</returns>
        public static Boolean IsServiceExist(String serviceName)
        {
            var services = ServiceController.GetServices().ToList();

            return services.Exists(s => String.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region 安装Windows服务

        /// <summary>
        /// 安装Windows服务
        /// </summary>
        /// <param name="filepath">服务exe文件路径</param>
        /// <param name="stateSaver">状态集, 默认为null</param>
        public static void InstallService(String filepath, IDictionary stateSaver = null)
        {
            AssemblyInstaller installer = new AssemblyInstaller();
            installer.UseNewContext = true;
            installer.Path = filepath;
            installer.Install(stateSaver);
            installer.Commit(stateSaver);
            installer.Dispose();
        }

        #endregion

        #region 卸载Windows服务

        /// <summary>
        /// 卸载Windows服务
        /// </summary>
        /// <param name="filepath">服务exe文件路径</param>
        public static void UnInstallService(String filepath)
        {
            AssemblyInstaller installer = new AssemblyInstaller();
            installer.UseNewContext = true;
            installer.Path = filepath;
            installer.Uninstall(null);
            installer.Dispose();
        }

        #endregion

        #region 判断window服务是否启动

        /// <summary>
        /// 判断某个Windows服务是否已经开启
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>服务是否已经开启</returns>
        public static Boolean IsServiceStart(String serviceName)
        {
            ServiceController controller = new ServiceController(serviceName);

            return controller.Status.Equals(ServiceControllerStatus.Running)
                || controller.Status.Equals(ServiceControllerStatus.StartPending);
        }

        #endregion

        #region 启动服务

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="waitSeconds">启动等待秒数, 默认为5</param>
        /// <returns>是否开启成功</returns>
        public static Boolean StartService(String serviceName, Int32 waitSeconds = 5)
        {
            //服务是否存在
            if (IsServiceExist(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);

                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    try
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, waitSeconds));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("服务器启动超时或失败!", ex);
                    }
                }
            }

            return true;
        }

        #endregion

        #region 停止服务

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="waitSeconds">启动等待秒数, 默认为5</param>
        /// <returns>服务是否成功停止</returns>
        public static Boolean StopService(String serviceName, Int32 waitSeconds = 5)
        {
            //服务是否存在
            if (IsServiceExist(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);

                if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
                {
                    try
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, waitSeconds));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("服务器停止超时或失败!", ex);
                    }
                }
            }

            return true;
        }

        #endregion
    }
}