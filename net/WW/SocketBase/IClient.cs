using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBase
{
    public interface IClient
    {
        /// <summary>
        /// 客户端发送文件以外的内容
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="bytes"></param>
        void ClientSend(ContextType contextType, byte[] bytes);

        /// <summary>
        /// 一次性发送整个文件
        /// </summary>
        void ClientSendFile(string filePath);

        /// <summary>
        /// 循环逐步发送文件
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        void ClientSendFileLoop(string filePath);
    }
}
