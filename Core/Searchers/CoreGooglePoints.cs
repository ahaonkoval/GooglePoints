using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Helpers;
using NLog;
using Core.Objects;

namespace Core.Searchers
{
    public class CoreGooglePoints
    {
        HelperDB hdb;
        HelperGoogle hgoogle;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public CoreGooglePoints()
        {
            hdb = new HelperDB();
            hgoogle = new HelperGoogle();
        }
        public void GetGooleCoordinates() {
            PointMap Point = hdb.GetEpicentrKPointForGeocoding();
            _logger.Info(string.Format("{0} {1} {2}", Point.CardId.ToString(), "Address Get DB:", Point.SourceAddress));

            Point = hgoogle.GetRequestXmlDocumentPointsByAddress(Point);
            _logger.Info(string.Format("{0} SearchEngineStatus:{5}, {1} {2} Lat={3}, Lng={4}", 
                Point.CardId.ToString(), 
                "Point Get Googe:", 
                Point.FormattedAddress, 
                Point.Coordinate != null ? Point.Coordinate.Lat : "none", 
                Point.Coordinate != null ? Point.Coordinate.Lng : "none",
                Point.Status.ToString()
            ));
            if (Point.CardId > 0)
            {
                Point.Save(hdb);
                _logger.Info(string.Format("{0} {1}", Point.CardId.ToString(), "Point Set Data in DB"));
            }
        }
    }
}


