using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * работаем с данными по Новой линии (адреса, точки и так далее...)
 */
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
