using Core.Helpers;
using Core.Objects;
using Core.ObjectSerializer;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Searchers
{
    public class OsmPointsSearcher : IDisposable
    {

        HelperDB hdb;
        //HelperGoogle hgoogle; OsmPointsSearcher
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public OsmPointsSearcher()
        {
            hdb = new HelperDB();
        }

        public OsmPointsSearcher(HelperDB _hdb)
        {
            if (_hdb == null)
            {
                hdb = new HelperDB();
            } else
            {
                hdb = _hdb;
            }
        }

        public ProcessPoint IdentifyCoordinatePoint(ProcessPoint point)
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
            string url = string.Format("{0}{1}&format=xml&addressdetails=1", ep, point.SourceAddress);

            string st = System.Text.Encoding.UTF8.GetString(client.DownloadData(url));

            XmlSerializer serializer = new XmlSerializer(typeof(Searchresults));

            using (TextReader tr = new StringReader(st))
            {
                Searchresults result = (Searchresults)serializer.Deserialize(tr);
                if (result.Place != null)
                {
                    point.Coordinate = new Coordinate(result.Place.Lat, result.Place.Lon);
                    point.FormattedAddress = result.Place.Display_name;
                    point.SetSearchEngineStatus("OK");
                    point.Conteiner = (object)result.Place;
                    point.SearchEngine = SearchEngine.Osm;
                    //point.PStatus = ProcessStatus.;
                }
                else
                {
                    point.SetSearchEngineStatus("ZERO_RESULTS");
                }

                XmlSerializer smr = new XmlSerializer(typeof(Searchresults));
                using (StringWriter tw = new StringWriter())
                {
                    smr.Serialize(tw, result);
                    point.Xml = tw.ToString();
                }

                _logger.Info(string.Format("{0} SearchEngineStatus:{5}, {1} {2} Lat={3}, Lng={4}",
                    point.CardId.ToString(),
                    "Point Get OSM:",
                    point.FormattedAddress,
                    point.Coordinate != null ? point.Coordinate.Lat : "none",
                    point.Coordinate != null ? point.Coordinate.Lng : "none",
                    point.PStatus.ToString()
                ));

                point.Save(hdb);

                _logger.Info(string.Format("{0} {1}", point.CardId.ToString(), "Point Set Data in DB"));

                return point;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~CoreOsmPoints() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
