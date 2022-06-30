using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeworkTheme09ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread task = new Thread(ChatBotWork.StartWork);
            task.Start();
        }
    }
}
