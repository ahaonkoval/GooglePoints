using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;

using Core.Objects;
using Core.Helpers;

namespace Core.Objects
{
    public class ProcessPoint
    {
        /// <summary>
        /// ИД точки в БД
        /// </summary>
        public long PointId { get; set; }
        /// <summary>
        /// тип точки
        /// </summary>
        public PointType Type { get; set; }
        /// <summary>
        /// Картка до якої прив'язана точка
        /// </summary>
        public long CardId { get; set; }
        /// <summary>
        /// УПЛ ИД
        /// </summary>
        public long CrmCustomerId { get; set; }
        /// <summary>
        /// Географвчні координати
        /// </summary>
        public Coordinate Coordinate { get; set; }
        /// <summary>
        /// адреса для пошукового запиту
        /// </summary>
        public string SourceAddress { get; set; }
        /// <summary>
        /// адреса - ф форматы выдповыды пошукового засобу
        /// </summary>
        public string FormattedAddress { get; set; }
        /// <summary>
        /// Статус обробки точки
        /// </summary>
        public ProcessStatus PStatus
        {
            get; set;
        }
        /// <summary>
        /// Статус пошукової машини
        /// </summary>
        private string SearchEngineStatus { get; set; }

        public SearchEngine SearchEngine { get; set; }
        /// <summary>
        /// відповідь в форматі xml від пошукового засобу
        /// </summary>
        public string Xml { get; set; }
        /// <summary>
        /// помилка
        /// </summary>
        public string Error { get; set; }

        public Object Conteiner { get; set; }
        public ProcessPoint() {
            this.Coordinate = null;
        }   
        public ProcessPoint(
                long pointId, long cardId, long crmCustomerId, Coordinate coordinate, string source_address, string formatted_address)
        {
            this.PointId = pointId;
            this.CardId = cardId;
            this.CrmCustomerId = crmCustomerId;
            this.Coordinate = coordinate;
            this.SourceAddress = source_address;
            this.FormattedAddress = formatted_address;
        }
        public void SetSearchEngineStatus(string status)
        {
            this.SearchEngineStatus = status;
        }

        public string GetSearchEngineStatus()
        {
            return this.SearchEngineStatus;
        }
        public void Save(HelperDB helper)
        {
            switch (this.Type) {
                case PointType.CustomerEpicentrK: helper.SetEpicentrKPoint(this);
                    break;
                case PointType.CustomerNewLine: helper.SetNewLinePoint(this);
                    break;
                case PointType.BoardPoint:
                    break;
                case PointType.MarketPoint:
                    break;
            }            
        }
    }

    public class Coordinate
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public Coordinate() { }

        public Coordinate(string lat, string lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }
    }

    public enum SearchEngine {
        Google = 0,
        Osm = 1,
        Yzndex = 2
    }
}
