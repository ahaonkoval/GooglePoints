using Core.Objects;
using Core.ObjectSerializer;
//using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Helpers
{
    public class HelperOpenRoadMaps
    {
        public void Get(ProcessPoint point)
        {

            WebClientExtended client = new WebClientExtended();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["isProxy"]))
            {
                string Host = ConfigurationManager.AppSettings["ProxyHost"];
                int Port = Convert.ToInt32(ConfigurationManager.AppSettings["ProxyPort"]);
                System.Net.WebProxy wp = new System.Net.WebProxy(Host, Port);
                client.Proxy = wp; 
            }

            string ep = string.Format(ConfigurationManager.AppSettings["OSMApiPath"]);
            string url = string.Format("{0}{1}&format=xml", ep, point.SourceAddress);

            string st = System.Text.Encoding.UTF8.GetString(client.DownloadData(url));
            XmlSerializer serializer = new XmlSerializer(typeof(Searchresults));

            using (TextReader tr = new StringReader(st))
            {
                Searchresults result = (Searchresults)serializer.Deserialize(tr);


            }
        }
    }
}
