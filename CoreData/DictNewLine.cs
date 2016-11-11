using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreData
{
    public class DictNewLine
    {
        public string DbConnectString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnectString"];
            }
        }
    }
}
