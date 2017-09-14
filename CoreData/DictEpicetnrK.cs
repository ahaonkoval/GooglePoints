using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Core;
using Core.Objects;
using LinqToDB;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;

/*
 * работаем с данными по Епицентру (адреса, точки и так далее...)
 */
namespace CoreData
{
    public class DictEpicetnrK: IDisposable
    {
        public string DbConnectString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnectString"];
            }
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataModels.DictTradeMarkets> GetMarkets()
        {
            using (var db = new DataModels.GeolocationDB())
            {
                return db.DictTradeMarkets.Where(w => w.TradingNetwork == "EpicentrK" && w.PotamusMarketId != null).ToList();
            }

            //List<Market> markets = new List<Market>();

            //string cmd_text = @"
            //        SELECT [market_google_coordinates_id]
            //              ,name_short
            //              ,[address]
            //              ,[lat]
            //              ,[lng]
            //              ,[label]
            //        FROM [dbo].[google_market_coordinates]
            //    ";

            //using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            //{
            //    SqlCommand cmd = connection.CreateCommand();
            //    cmd.CommandText = cmd_text;
            //    cmd.CommandType = CommandType.Text;

            //    connection.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        markets.Add(new Market
            //        {
            //            market_google_coordinates_id = Convert.ToInt32(reader[0]),
            //            name_short = reader[1].ToString().Trim(),
            //            address = reader[2].ToString(),
            //            lat = Convert.ToDecimal(reader[3]),
            //            lng = Convert.ToDecimal(reader[4]),
            //            label = Convert.ToString(reader[5])
            //        });
            //    }
            //    connection.Close();
            //}
            //return markets;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataModels.DictTradeMarkets GetMarketById(long id)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                return db.DictTradeMarkets.Where(w => w.MarketId == id).FirstOrDefault();
            }

            //Market market = new Market();

            //string cmd_text = @"
            //        SELECT [market_google_coordinates_id]
            //              ,name_short
            //              ,[address]
            //              ,[lat]
            //              ,[lng]
            //              ,[label]
            //        FROM [dbo].[google_market_coordinates] where market_google_coordinates_id = {0}
            //    ";
            //cmd_text = string.Format(cmd_text, id.ToString());

            //using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            //{
            //    SqlCommand cmd = connection.CreateCommand();
            //    cmd.CommandText = cmd_text;
            //    cmd.CommandType = CommandType.Text;

            //    connection.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        market = new Market
            //        {
            //            market_google_coordinates_id = Convert.ToInt32(reader[0]),
            //            name_short = reader[1].ToString().Trim(),
            //            address = reader[2].ToString(),
            //            lat = Convert.ToDecimal(reader[3]),
            //            lng = Convert.ToDecimal(reader[4]),
            //            label = Convert.ToString(reader[5])
            //        };
            //    }
            //    connection.Close();
            //}
            //return market;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="market_id"></param>
        /// <returns></returns>
        public IEnumerable<CardPoint> GetPointsByMarketId(int market_id)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                //var market = db.DictTradeMarkets.Where(w => w.MarketId == market_id).FirstOrDefault();

                return db.VCustomerPositions.Where(w => w.IssuedMarketId == market_id && w.SearchEngineStatus == "OK")
                    .Select(x => new CardPoint {
                        formatted_address = x.FormattedAddress,
                        lat = x.Lat.Value,
                        lng = x.Lng.Value,
                        point_id = x.PointId.Value,
                        search_engine_status = x.SearchEngineStatus
                });
            }
            /*
            List<CardPoint> cardpoints = new List<CardPoint>();
            string cmd_text = @"
                    declare
                        @potamus_market_id  bigint; 
                    declare
                        @visited            int                    

                    select top 1 
                        @potamus_market_id = potamus_market_id 
                    from [dbo].[google_market_coordinates] where market_google_coordinates_id={0}

                    set @visited = {1}                    

                    select 
	                    b.[google_point_id]
                        ,b.[lat]
                        ,b.[lng]
                        ,b.[google_status]
                        ,isnull(b.[formatted_address],'') formatted_address
                    from [dbo].[v_card_customers_true] a 
						left join [dbo].[google_card_points] b on a.card_id = b.card_id
						inner join dbo.v_card_visited vc on a.card_id = vc.card_id
                    where 
	                    b.[google_status] = 'OK'
	                    and a.issued_market_id = @potamus_market_id
						and vc.visit >= iif(@visited = 0, vc.visit, @visited)
                ";
            cmd_text = string.Format(cmd_text, market_id.ToString(), visited.ToString());
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cardpoints.Add(new CardPoint
                    {
                        google_point_id = Convert.ToInt32(reader[0]),
                        lat = Convert.ToDecimal(reader[1]),
                        lng = Convert.ToDecimal(reader[2]),
                        google_status = reader[3].ToString(),
                        formatted_address = reader[4].ToString()
                    });
                }
                connection.Close();
            }
            return cardpoints;
            */
        }

        public IEnumerable<CardPoint> GetPointsByMarketRadius(int market_id, int radius)
        {
            List<CardPoint> cp = new List<CardPoint>();
            using (var db = new DataModels.GeolocationDB())
            {
                LinqToDB.Data.DataConnection connect = new LinqToDB.Data.DataConnection();
                IEnumerable<DataModels.GeolocationDBStoredProcedures.PGetPointsByMarketRadiusResult> re = 
                    (IEnumerable<DataModels.GeolocationDBStoredProcedures.PGetPointsByMarketRadiusResult>)
                        DataModels.GeolocationDBStoredProcedures.PGetPointsByMarketRadius(connect, market_id, radius).ToList();
                if (re.Count() > 0)
                {                   
                    foreach (DataModels.GeolocationDBStoredProcedures.PGetPointsByMarketRadiusResult r in re)
                    {
                        cp.Add(new CardPoint {
                            formatted_address = r.formatted_address,
                            lat = r.lat.Value,
                            lng = r.lng.Value,
                            point_id = r.point_id.Value,
                            search_engine_status = r.search_engine_status
                        });
                    }                   
                }
                return cp;
            }

             
            //using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            //{
            //    SqlCommand cmd = connection.CreateCommand();
            //    cmd.CommandText = "dbo.p_get_points_by_market_stat";
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.Add("@market_google_coordinates_id", SqlDbType.Int).Value = market_id;
            //    cmd.Parameters.Add("@radius", SqlDbType.Int).Value = radius;

            //    connection.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        cardpoints.Add(new CardPoint
            //        {
            //            google_point_id = Convert.ToInt32(reader[0]),
            //            lat = Convert.ToDecimal(reader[1]),
            //            lng = Convert.ToDecimal(reader[2]),
            //            google_status = reader[3].ToString(),
            //            formatted_address = reader[4].ToString()
            //        });
            //    }
            //    connection.Close();
            //}
            //return cardpoints;
        }

        public int GetCountPointsByMarketRadius(int market_id, int radius)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                LinqToDB.Data.DataConnection connect = new LinqToDB.Data.DataConnection();
                DataModels.GeolocationDBStoredProcedures.PGetCountPointsByMarketRadiusResult re =
                    DataModels.GeolocationDBStoredProcedures.PGetCountPointsByMarketRadius(connect, market_id, radius).FirstOrDefault();

                return re.qty.Value;
            }

            //int CardCount = 0;
            //using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            //{
            //    SqlCommand cmd = connection.CreateCommand();
            //    cmd.CommandText = "dbo.p_get_count_points_by_market_stat";
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.Add("@market_google_coordinates_id", SqlDbType.Int).Value = market_id;
            //    cmd.Parameters.Add("@radius", SqlDbType.Int).Value = radius;

            //    connection.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        CardCount = Convert.ToInt32(reader[0]);
            //    }
            //    connection.Close();
            //}

            //return CardCount;
        }

        //public List<Dict> GetSegmentByVisited()
        //{
        //    List<Dict> visits = new List<Dict>();
        //    using (SqlConnection connection = new SqlConnection(this.DbConnectString))
        //    {
        //        SqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandText = "SELECT [id],[name],[start],[end] FROM dbo.dict_segment_visited order by id";
        //        cmd.CommandType = CommandType.Text;

        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            var o = new Dict
        //            {
        //                id = Convert.ToInt32(reader[0]),
        //                name = Convert.ToString(reader[1])
        //            };
        //            visits.Add(o);
        //        }
        //        connection.Close();
        //    }
        //    return visits.OrderByDescending(m=> m.id).ToList();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<CardPoint> GetPointsCustomerUsedViber()
        //{
        //    List<CardPoint> cardpoints = new List<CardPoint>();
        //    using (SqlConnection connection = new SqlConnection(this.DbConnectString))
        //    {
        //        SqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandText = @"
        //            select 
        //                c.google_point_id,
	       //             c.lat,
	       //             c.lng,
        //                c.google_status,
	       //             c.formatted_address
        //            from [dbo].[crm_isviber] a 
        //            inner join [dbo].[v_card_customers_true] b on a.card_id = b.card_id
        //            inner join geolocation..google_card_points c on a.card_id = c.card_id
        //            where
	       //             b.name_city is not null
	       //             and c.google_status = 'OK'
        //        ";
        //        cmd.CommandType = CommandType.Text;

        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            cardpoints.Add(new CardPoint
        //            {
        //                google_point_id = Convert.ToInt32(reader[0]),
        //                lat = Convert.ToDecimal(reader[1]),
        //                lng = Convert.ToDecimal(reader[2]),
        //                google_status = reader[3].ToString(),
        //                formatted_address = reader[4].ToString()
        //            });
        //        }
        //        connection.Close();
        //    }
        //    return cardpoints;
        //}

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
