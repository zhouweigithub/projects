// ****************************************************
// FileName:FormatCodeBLL.cs
// Description:
// Tables:
// Author:ZhouWei
// Create Date:2020-04-17
// Revision History:
// ****************************************************
using System;
using System.IO;

namespace FormatCode
{
    public static class FormatCodeBLL
    {

        public static void Do()
        {
            Console.WriteLine("FORMAT CODE");

            Console.Write("INPUT FOLDER ");

            String folder = Console.ReadLine();

            folder = folder.Trim();

            if (String.IsNullOrEmpty(folder))
            {
                Console.WriteLine("INPUT ERROR");
            }
            else
            {
                Do(folder);
            }

            Console.Write("PRESS ANY KEY TO CONTINUE...");

            Console.ReadKey();
        }

        /// <summary>
        /// 格式化代码
        /// </summary>
        /// <param name="path"></param>
        public static void Do(String path)
        {
            try
            {
                if (String.IsNullOrEmpty(path))
                {
                    Console.WriteLine("FOLDER EMPTY");

                    return;
                }

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("FOLDER NOT EXIST");

                    return;
                }


                ExegesisBLL.Do(path);

                MoveUsingBLL.Do(path);

                BreakLineBLL.Do(path);

                Console.WriteLine("OVER");
            }
            catch (Exception e)
            {
                Console.WriteLine();

                Console.WriteLine(e.ToString());

                Console.WriteLine();
            }
        }
    }
}
