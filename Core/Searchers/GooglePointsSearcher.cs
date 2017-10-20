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
    public class GooglePointsSearcher: IDisposable
    {
        HelperDB hdb;
        HelperGoogle hgoogle;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public GooglePointsSearcher()
        {
            hdb = new HelperDB();
            hgoogle = new HelperGoogle();
        }

        public GooglePointsSearcher(ProcessPoint point)
        {
            hdb = new HelperDB();
            hgoogle = new HelperGoogle();
        }

        public ProcessPoint IdentifyCoordinatePoint(ProcessPoint point)
        {
            if (GetRemainingAttemptsCount() <= 0) {
                point.PStatus = ProcessStatus.ATTEMTS_TO_END;
                return point;
            }

            _logger.Info(string.Format("{0} {1} {2}", point.CardId.ToString(), "Address Get DB:", point.SourceAddress));

            point = hgoogle.GetRequestXmlDocumentPointsByAddress(point);

            _logger.Info(string.Format("{0} SearchEngineStatus:{5}, {1} {2} Lat={3}, Lng={4}",
                point.CardId.ToString(),
                "Point Get Googe:",
                point.FormattedAddress,
                point.Coordinate != null ? point.Coordinate.Lat : "none",
                point.Coordinate != null ? point.Coordinate.Lng : "none",
                point.PStatus.ToString()
            ));

            if (point.CardId > 0)
            {
                point.Save(hdb);
                _logger.Info(string.Format("{0} {1}", point.CardId.ToString(), "Point Set Data in DB"));
            }

            return point;          
        }
        /// <summary>
        /// 
        /// </summary>
        public void IdentifyCoordinatesOne(PointType pt)
        {
            if (GetRemainingAttemptsCount() <= 0) return;

            switch (pt)
            {
                case PointType.CustomerEpicentrK:
                    {
                        ProcessPoint point = hdb.GetEpicentrKPointForGeocoding();

                        _logger.Info(string.Format("{0} {1} {2}", point.CardId.ToString(), "Address Get DB:", point.SourceAddress));

                        point = hgoogle.GetRequestXmlDocumentPointsByAddress(point);

                        _logger.Info(string.Format("{0} SearchEngineStatus:{5}, {1} {2} Lat={3}, Lng={4}",
                            point.CardId.ToString(),
                            "Point Get Googe:",
                            point.FormattedAddress,
                            point.Coordinate != null ? point.Coordinate.Lat : "none",
                            point.Coordinate != null ? point.Coordinate.Lng : "none",
                            point.PStatus.ToString()
                        ));

                        if (point.CardId > 0)
                        {
                            point.Save(hdb);
                            _logger.Info(string.Format("{0} {1}", point.CardId.ToString(), "Point Set Data in DB"));
                        }
                        break;
                    }
                case PointType.BoardPoint:
                    {
                        break;
                    }
                case PointType.CustomerNewLine:
                    {
                        break;
                    }
                case PointType.MarketPoint:
                    {
                        break;
                    }
            }
        }

        int GetRemainingAttemptsCount()
        {
            return hdb.GetRemainingAttemptsCount();
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
        // ~GooglePointsSearcher() {
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


