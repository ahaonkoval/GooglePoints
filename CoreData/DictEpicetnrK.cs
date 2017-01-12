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
        public long SetDistrict(long potamus_district_id, long region_id, string name, string poligon)
        {
            long district_id = 0;

            string cmd_text = @"
                INSERT INTO [dbo].[dict_districts]
                           ([potamus_district_id]
                           ,[region_id]
                           ,[name]
                           ,[poligon])
                     VALUES
                           (@potamus_district_id
                           ,@region_id
                           ,@name 
                           ,geography::STPolyFromText('POLYGON((%poligon%))', 4326))
                SELECT scope_identity() id
                ";

            cmd_text = cmd_text.Replace("%poligon%", poligon);
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@potamus_district_id", potamus_district_id);
                cmd.Parameters.AddWithValue("@region_id", region_id);
                cmd.Parameters.AddWithValue("@name", name);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable tb = new DataTable();
                tb.Load(reader);
                reader.Close();
                connection.Close();

                district_id = Convert.ToInt64(tb.Rows[0]["id"]);
            }

            return district_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="district_id"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void SetDistrictCoordinates(long district_id, decimal lat, decimal lng)
        {
            string cmd_text = @"
                INSERT INTO [dbo].[dict_districts_coordinates]
                           ([lat]
                           ,[lng]
                           ,[district_id])
                     VALUES
                           (@lat
                           ,@lng
                           ,@district_id)
                ";
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@district_id", district_id);
                cmd.Parameters.AddWithValue("@lat", lat);
                cmd.Parameters.AddWithValue("@lng", lng);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Region> GetRegions()
        {
            List<Region> regions = new List<Region>();

            string cmd_text = @"
                SELECT [region_id]
                      ,[country_id]
                      ,[name]
                  FROM [dbo].[dict_regions]
                ";
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    regions.Add(new Region {
                        region_id = Convert.ToInt32(reader[0]),
                        country_id = reader[1].ToString(),
                        name = reader[2].ToString()
                    });
                }
                connection.Close();
            }
            return regions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<District> GetDistrictsByRegionId(long id)
        {
            List<District> districts = new List<District>();

            string cmd_text = @"
                        SELECT 
	                        [district_id]
                            ,[potamus_district_id]
                            ,[region_id]
                            ,[name]
                            ,[poligon].EnvelopeCenter().Long as lat
                            ,[poligon].EnvelopeCenter().Lat  as lng
                            ,is_centre_region
                        FROM [dbo].[dict_districts] where [region_id] = {0}
                ";
            cmd_text = string.Format(cmd_text, id.ToString());

            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    districts.Add(new District
                    {
                        district_id = Convert.ToInt32(reader[0]),
                        potamus_district_id = Convert.ToInt32(reader[1]),
                        region_id = Convert.ToInt32(reader[2]),
                        name = reader[3].ToString(),
                        lat = Convert.ToDecimal(reader[4]),
                        lng = Convert.ToDecimal(reader[5]),
                        is_region_center = Convert.ToBoolean(reader[6])
                    });
                }
                connection.Close();
            }
            return districts;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public District GetDistrictById(long id)
        {
            District district = new District();

            string cmd_text = @"
                        SELECT Top 1
	                        [district_id]
                            ,[potamus_district_id]
                            ,[region_id]
                            ,[name]
                            ,[poligon].EnvelopeCenter().Long as lat
                            ,[poligon].EnvelopeCenter().Lat  as lng
                        FROM [dbo].[dict_districts] where [district_id] = {0}
                ";
            cmd_text = string.Format(cmd_text, id.ToString());

            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    district = new District
                    {
                        district_id = Convert.ToInt32(reader[0]),
                        potamus_district_id = Convert.ToInt32(reader[1]),
                        region_id = Convert.ToInt32(reader[2]),
                        name = reader[3].ToString(),
                        lat = Convert.ToDecimal(reader[4]),
                        lng = Convert.ToDecimal(reader[5])
                    };
                }
                connection.Close();
            }
            return district;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<DistrictCoordinate> GetDistrictCoordinates(long id)
        {
            List<DistrictCoordinate> Coordinates = new List<DistrictCoordinate>();

            string cmd_text = @"
                    SELECT 
	                    [coordinate_id]
                        ,[lat]
                        ,[lng]
                        ,[district_id]
                    FROM [dbo].[dict_districts_coordinates] where district_id = {0}
                ";
            cmd_text = string.Format(cmd_text, id.ToString());

            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Coordinates.Add(new DistrictCoordinate
                    {
                        coordinate_id = Convert.ToInt32(reader[0]),
                        lat = Convert.ToDecimal(reader[1]),
                        lng = Convert.ToDecimal(reader[2]),
                        district_id = Convert.ToInt64(reader[3])
                    });
                }
                connection.Close();
            }
            return Coordinates;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Market> GetMarkets()
        {
            List<Market> markets = new List<Market>();

            string cmd_text = @"
                    SELECT [market_google_coordinates_id]
                          ,name_short
                          ,[address]
                          ,[lat]
                          ,[lng]
                          ,[label]
                    FROM [dbo].[google_market_coordinates]
                ";

            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    markets.Add(new Market
                    {
                        market_google_coordinates_id = Convert.ToInt32(reader[0]),
                        name_short = reader[1].ToString().Trim(),
                        address = reader[2].ToString(),
                        lat = Convert.ToDecimal(reader[3]),
                        lng = Convert.ToDecimal(reader[4]),
                        label = Convert.ToString(reader[5])
                    });
                }
                connection.Close();
            }
            return markets;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Market GetMarketById(long id)
        {
            Market market = new Market();

            string cmd_text = @"
                    SELECT [market_google_coordinates_id]
                          ,name_short
                          ,[address]
                          ,[lat]
                          ,[lng]
                          ,[label]
                    FROM [dbo].[google_market_coordinates] where market_google_coordinates_id = {0}
                ";
            cmd_text = string.Format(cmd_text, id.ToString());

            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = cmd_text;
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    market = new Market
                    {
                        market_google_coordinates_id = Convert.ToInt32(reader[0]),
                        name_short = reader[1].ToString().Trim(),
                        address = reader[2].ToString(),
                        lat = Convert.ToDecimal(reader[3]),
                        lng = Convert.ToDecimal(reader[4]),
                        label = Convert.ToString(reader[5])
                    };
                }
                connection.Close();
            }
            return market;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="market_id"></param>
        /// <returns></returns>
        public IEnumerable<CardPoint> GetPointsByMarketId(int market_id, int visited)
        {
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
        }

        public IEnumerable<CardPoint> GetPointsByMarketStat(int market_id, int radius)
        {
            List<CardPoint> cardpoints = new List<CardPoint>();
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.p_get_points_by_market_stat";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@market_google_coordinates_id", SqlDbType.Int).Value = market_id;
                cmd.Parameters.Add("@radius", SqlDbType.Int).Value = radius;

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
        }

        public int GetCountPointsByMarketStat(int market_id, int radius)
        {
            int CardCount = 0;
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.p_get_count_points_by_market_stat";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@market_google_coordinates_id", SqlDbType.Int).Value = market_id;
                cmd.Parameters.Add("@radius", SqlDbType.Int).Value = radius;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CardCount = Convert.ToInt32(reader[0]);
                }
                connection.Close();
            }

            return CardCount;
        }

        public List<Dict> GetSegmentByVisited()
        {
            List<Dict> visits = new List<Dict>();
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT [id],[name],[start],[end] FROM dbo.dict_segment_visited order by id";
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var o = new Dict
                    {
                        id = Convert.ToInt32(reader[0]),
                        name = Convert.ToString(reader[1])
                    };
                    visits.Add(o);
                }
                connection.Close();
            }
            return visits.OrderByDescending(m=> m.id).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CardPoint> GetPointsCustomerUsedViber()
        {
            List<CardPoint> cardpoints = new List<CardPoint>();
            using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    select 
                        c.google_point_id,
	                    c.lat,
	                    c.lng,
                        c.google_status,
	                    c.formatted_address
                    from [dbo].[crm_isviber] a 
                    inner join [dbo].[v_card_customers_true] b on a.card_id = b.card_id
                    inner join geolocation..google_card_points c on a.card_id = c.card_id
                    where
	                    b.name_city is not null
	                    and c.google_status = 'OK'
                ";
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
        }

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
