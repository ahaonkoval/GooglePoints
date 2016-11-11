using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceDebuggerHelper;

namespace CoordinateFinder
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
#if DEBUG
            Application.Run(new ServiceRunner(new ServiceFinder()));
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceFinder()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
