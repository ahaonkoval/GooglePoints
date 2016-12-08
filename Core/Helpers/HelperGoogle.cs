using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using System.Spatial;
using NLog;
using Core.Objects;
using Core.ObjectSerializer;

namespace Core.Helpers
{
    public class HelperGoogle
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public PointMap GetRequestXmlDocumentPointsByAddress(PointMap Point)
        {
            try
            {
                if (Point.CardId > 0)
                {
                    string key = ConfigurationManager.AppSettings["GoogleKey"];
                    string GoogleApiPath = string.Format(ConfigurationManager.AppSettings["GoogleApiPath"], Point.SourceAddress, "&", key);

                    string Host = ConfigurationManager.AppSettings["ProxyHost"];
                    int  Port = Convert.ToInt32(ConfigurationManager.AppSettings["ProxyPort"]);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(GoogleApiPath);

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["isProxy"]))
                    {
                        System.Net.WebProxy wp = new System.Net.WebProxy(Host, Port);
                        request.Proxy = wp;
                    }
                    
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response.GetResponseStream();

                    XmlSerializer serializer = new XmlSerializer(typeof(GeocodeResponse));
                    GeocodeResponse message = (GeocodeResponse)serializer.Deserialize(dataStream);
                    return ProcessingRequestResult(Point, message);
                }
                else
                {
                    return Point;
                }
            }
            catch (Exception ex)
            {
                SetError("Ошибка получения данных google", ex);
                return Point;
            }
            finally
            {
                
            }
        }

        private PointMap ProcessingRequestResult(PointMap Point, GeocodeResponse message)
        {
            GoogleStatus status = (GoogleStatus)Enum.Parse(typeof(GoogleStatus), message.status);
            //PointMap Point = null;
            Coordinate GeoPoint = null;

            string formatted_address = string.Empty;

            if (status == GoogleStatus.OK) {
                if (message.result.Count() == 1)
                {
                    GeoPoint = new Coordinate(
                        message.result[0].geometry[0].location[0].lat,
                        message.result[0].geometry[0].location[0].lng);
                    formatted_address = message.result[0].formatted_address;
                }
                else
                {
                    status = GoogleStatus.MORE_ONE_POINT;
                }
            }

            Point = new PointMap(Point.CardId, GeoPoint, Point.SourceAddress, formatted_address, status);
            Point.MarketType = MarketType.EpicentrK;
            XmlSerializer serializer = new XmlSerializer(typeof(GeocodeResponse));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, message);
                Point.Xml = textWriter.ToString();
            }
            return Point;
        }

        private void SetError(string ErrorType, Exception ex)
        {
            _logger.Error(string.Format("{0}: {1}", ErrorType, ex.Message));
        }
    }
}

