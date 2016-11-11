using Core.Objects;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    
    public class HelperYandex
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PointMap GetRequestXmlDocumentPointsByAddress(PointMap Point)
        {
            try
            {
                if (Point.CardId > 0)
                {
                    //string key = ConfigurationManager.AppSettings["GoogleKey"];
                    string GoogleApiPath = string.Format(ConfigurationManager.AppSettings["YandexApiPath"], Point.SourceAddress);

                    string Host = ConfigurationManager.AppSettings["ProxyHost"];
                    int Port = Convert.ToInt32(ConfigurationManager.AppSettings["ProxyPort"]);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(GoogleApiPath);

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["isProxy"]))
                    {
                        System.Net.WebProxy wp = new System.Net.WebProxy(Host, Port);
                        request.Proxy = wp;
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response.GetResponseStream();

                    using (StreamReader reader = new StreamReader(dataStream, Encoding.UTF8))
                    {
                        string xml = string.Empty;
                        while (reader.Peek() >= 0)
                        {
                            string line = reader.ReadLine();
                            if (line.Contains("<pos>"))
                            {
                                string[] position = line.Replace("<pos>", "").Replace("</pos>", "").Trim().Split(' ');

                                if (Point.Coordinate == null)
                                    Point.Coordinate = new Coordinate(position[1], position[0]);

                                //if (Point.Coordinate.Lat == string.Empty)
                                //    Point.Coordinate.Lat = position[1];
                                //if (Point.Coordinate.Lng == string.Empty)
                                //    Point.Coordinate.Lng = position[0];
                            }
                            if (line.Contains("AddressLine"))
                            {
                                string addres = line.Replace("<AddressLine>", "").Replace("</AddressLine>", "").Trim();
                                if (Point.FormattedAddress == null)
                                {
                                    Point.FormattedAddress = addres;
                                }
                            }
                            xml = xml + line;
                        }
                        Point.Xml = xml;

                        if (Point.Coordinate != null)
                        {
                            Point.Status = GoogleStatus.OK;
                        } else {
                            Point.Status = GoogleStatus.ZERO_RESULTS;
                        }
                    }
                    return Point;
                }
                else
                {
                    return Point;
                }
            }
            catch (Exception ex)
            {
                SetError("Ошибка получения данных Yandex", ex);
                return Point;
            }
            finally
            {

            }
        }
        private void SetError(string ErrorType, Exception ex)
        {
            _logger.Error(string.Format("{0}: {1}", ErrorType, ex.Message));
        }
    }
}
