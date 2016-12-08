using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NLog;
using Core.Objects;

namespace Core.Helpers
{
    public class HelperDB
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        #region EpicentrK
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PointMap GetEpicentrKPointForGeocoding()
        {
            //TODO пишемо запит в БД через храниму процедуру gpo.get_unchecked_address
            PointMap Point = new PointMap();
            Point.MarketType = MarketType.EpicentrK;
            string DbConnectString = ConfigurationManager.AppSettings["DbConnectString"];

            using (SqlConnection connect = new SqlConnection(DbConnectString))
            {
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandText = "dbo.get_unchecked_address";
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable t = new DataTable();
                    t.Load(reader);
                    cmd.Connection.Close();
                    if (t.Rows.Count > 0) { 
                    Point.CardId = Convert.ToInt64(t.Rows[0]["card_id"]);
                    Point.SourceAddress = Convert.ToString(t.Rows[0]["source_address"]);}
                }
                catch (Exception ex)
                {
                    SetError("Ошибка получения адреса", ex);
                }
                finally
                {
                    if (connect.State != ConnectionState.Closed) connect.Close();
                }
            }

            return Point;
        }
        public void SetEpicentrKPoint(PointMap Point)
        {
            string DbConnectString = ConfigurationManager.AppSettings["DbConnectString"];

            using (SqlConnection connect = new SqlConnection(DbConnectString))
            {
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandText = "dbo.set_point_address";
                cmd.CommandType = CommandType.StoredProcedure;

                string Latitude = Point.Coordinate == null ? string.Empty : Point.Coordinate.Lat;
                string Longitude = Point.Coordinate == null ? string.Empty : Point.Coordinate.Lng;

                cmd.Parameters.AddWithValue("@card_id", Point.CardId);
                cmd.Parameters.AddWithValue("@lat", Latitude);
                cmd.Parameters.AddWithValue("@lng", Longitude);
                cmd.Parameters.AddWithValue("@google_status", Point.Status.ToString());
                cmd.Parameters.AddWithValue("@formatted_address", Point.FormattedAddress);
                cmd.Parameters.AddWithValue("@xml", Point.Xml);

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    SetError("Ошибка сохранения точки в БД", ex);
                }
                finally
                {
                    if (connect.State != ConnectionState.Closed) connect.Close();
                }
            }
        }
        #endregion

        #region New Line

        public PointMap GetNewLinePointForGeocoding()
        {
            PointMap Point = new PointMap();
            string DbConnectString = ConfigurationManager.AppSettings["DbConnectString"];
            using (SqlConnection connect = new SqlConnection(DbConnectString))
            {
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandText = "[nl].[get_unchecked_address]";
                cmd.CommandType = CommandType.Text;

                try
                {
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable t = new DataTable();
                    t.Load(reader);
                    cmd.Connection.Close();
                    if (t.Rows.Count > 0)
                    {
                        Point.CardId = Convert.ToInt64(t.Rows[0]["geo_customer_id"]);
                        Point.SourceAddress = Convert.ToString(t.Rows[0]["source_address"]);
                    }
                    cmd.Connection.Open();

                }
                catch (Exception ex)
                {
                    SetError("Ошибка получения адреса", ex);
                }
                finally
                {
                    if (connect.State != ConnectionState.Closed) connect.Close();
                }

            }

            return Point;
        }

        public void SetNewLinePoint(PointMap Point)
        {
            string DbConnectString = ConfigurationManager.AppSettings["DbConnectString"];

            using (SqlConnection connect = new SqlConnection(DbConnectString))
            {
                SqlCommand cmd = connect.CreateCommand();
                cmd.CommandText = "nl.set_point_address";
                cmd.CommandType = CommandType.StoredProcedure;

                string Latitude = Point.Coordinate == null ? string.Empty : Point.Coordinate.Lat;
                string Longitude = Point.Coordinate == null ? string.Empty : Point.Coordinate.Lng;

                cmd.Parameters.AddWithValue("@geo_customer_id", Point.CardId);
                cmd.Parameters.AddWithValue("@lat", Latitude);
                cmd.Parameters.AddWithValue("@lng", Longitude);
                cmd.Parameters.AddWithValue("@google_status", Point.Status.ToString());
                cmd.Parameters.AddWithValue("@formatted_address", Point.FormattedAddress == null ? "" : Point.FormattedAddress);
                cmd.Parameters.Add("@xml", SqlDbType.VarChar, 8000).Value = Point.Xml;

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    SetError("Ошибка сохранения точки в БД", ex);
                }
                finally
                {
                    if (connect.State != ConnectionState.Closed) connect.Close();
                }
            }
        }

        #endregion
        private void SetError(string ErrorType, Exception ex) 
        {
            _logger.Error(string.Format("{0}: {1}", ErrorType, ex.Message));
        }
    }
}
