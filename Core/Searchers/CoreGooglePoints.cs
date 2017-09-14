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
    public class CoreGooglePoints: IDisposable
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
            PointMap Point = hdb.GetEpicentrKPointForGeocoding(); //<-- Перейменувати та використовувати нову процедуру
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
        // ~CoreGooglePoints() {
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


