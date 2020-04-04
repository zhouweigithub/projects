// ****************************************
// FileName:CPUUtil.cs
// Description:CPU助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-01-27
// Revision History:
// ****************************************

using System;
using System.Management;

namespace Util.OS
{
    /// <summary>
    /// CPU助手类
    /// </summary>
    public static class CPUUtil
    {
        /// <summary>
        /// 获取物理处理的数量
        /// </summary>
        /// <returns>物理处理的数量</returns>
        public static Int32 GetPhysicalProcessorCount()
        {
            Int32 count = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                count = Int32.Parse(item["NumberOfProcessors"].ToString());
            }

            return count;
        }

        /// <summary>
        /// 获取逻辑处理器的数量
        /// </summary>
        /// <returns>逻辑处理器的数量</returns>
        public static Int32 GetLogicalProcessorCount()
        {
            Int32 count = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                count = Int32.Parse(item["NumberOfLogicalProcessors"].ToString());
            }

            return count;
        }

        /// <summary>
        /// 获取处理器核的数量
        /// </summary>
        /// <returns>处理器核的数量</returns>
        public static Int32 GetCoreCount()
        {
            Int32 count = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                count += Int32.Parse(item["NumberOfCores"].ToString());
            }

            return count;
        }
    }
}