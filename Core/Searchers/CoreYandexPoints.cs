using Core.Helpers;
using Core.Objects;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Searchers
{
    public class CoreYandexPoints
    {
        HelperDB hdb;
        HelperYandex hyandex;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public CoreYandexPoints()
        {
            hdb = new HelperDB();
            hyandex = new HelperYandex();
        }
        public void GetYandexCoordinates() {
            PointMap Point = hdb.GetNewLinePointForGeocoding();
            Point.MarketType = MarketType.NewLine;

            _logger.Info(string.Format("{0} {1} {2}", Point.CardId.ToString(), "Address Get DB for check Yandex:", Point.SourceAddress));

            Point = hyandex.GetRequestXmlDocumentPointsByAddress(Point);
            _logger.Info(string.Format("{0} YandexStatus:{5}, {1} {2} Lat={3}, Lng={4}",
                Point.CardId.ToString(),
                "Point Get Yandex:",
                Point.FormattedAddress,
                Point.Coordinate != null ? Point.Coordinate.Lat : "none",
                Point.Coordinate != null ? Point.Coordinate.Lng : "none",
                Point.Status.ToString()
            ));
            if (Point.CardId > 0)
            {
                Point.Save(hdb);
                _logger.Info(string.Format("{0} {1}", Point.CardId.ToString(), "Yandex Point Set Data in DB"));
            }

        }
    }
}
