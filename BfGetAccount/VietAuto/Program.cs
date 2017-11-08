using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VietAuto
{
    class Program
    {
        static void Main(string[] args)
        {
            var vietAutoClient = new VietAutoClient();
            var acccount = vietAutoClient.Login(ConsValue.DefaultUser, ConsValue.DefaultPass);
            var allAcccount = vietAutoClient.GetAllAcccount();
            foreach (var account in allAcccount)
            {
                Console.WriteLine(account);
            }
            Console.ReadKey();

        }
    }
}
