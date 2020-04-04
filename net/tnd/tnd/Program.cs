using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tnd
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 1 && (args[0] == "?" || args[0] == "-?"
                || args[0] == "-h" || args[0] == "-help"))
            {
                Console.WriteLine("file aes encrypt");
                Console.WriteLine("options");
                Console.WriteLine("-e sourcepath [savepath]    --encode");
                Console.WriteLine("-d sourcepath [savepath]    --decode");
                return;
            }


            if (args.Length < 2 && args.Length > 3)
            {
                Console.WriteLine("parameter incorect");
                return;
            }


            try
            {
                if (args[0] == "-e")
                {
                    if (args.Length == 2)
                    {
                        string result = BLL.EnCode(args[1]);
                        Console.WriteLine(result);
                    }
                    else
                    {
                        BLL.EnCode(args[1], args[2]);
                        Console.WriteLine("success");
                    }
                }
                else if (args[0] == "-d")
                {
                    if (args.Length == 2)
                    {
                        string result = BLL.DeCode(args[1]);
                        Console.WriteLine(result);
                    }
                    else
                    {
                        BLL.DeCode(args[1], args[2]);
                        Console.WriteLine("success");
                    }
                }
                else
                {
                    Console.WriteLine("parameter incorect");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
