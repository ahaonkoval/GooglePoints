using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Core;
//using Core.Objects;
using LinqToDB;
using Microsoft.SqlServer.Types;
using DataModels;

namespace CoreData
{
    public class DictGeoData: IDisposable
    {
        public string DbConnectString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnectString"];
            }
        }

        public int GetGoogleRemainingAttemptsCount()
        {
            using (var db = new GeolocationDB())
            {
                int AttemptQty = 0;
                long limitId = 0;
                db.BeginTransaction();
                var l = db.GoogleRequestLimit.Where(w => w.DateRequest.Value.ToString("yyyy-MM-ddd") == DateTime.Now.ToString("yyyy-MM-ddd")).FirstOrDefault();
                if (l == null)
                {
                    AttemptQty = 2500;
                    object id = db.GoogleRequestLimit.InsertWithIdentity(() => new GoogleRequestLimit
                    {
                        Limit = AttemptQty,
                        DateRequest = DateTime.Now
                    });
                    limitId = Convert.ToInt64(id);
                }
                else
                {
                    AttemptQty = l.Limit.Value;
                    limitId = l.GoogleRequestLimitId;
                }
                db.GoogleRequestLimit.Where(w => w.GoogleRequestLimitId == limitId)
                    .Set(p => p.Limit, AttemptQty - 1).Update();
                db.CommitTransaction();

                return AttemptQty;
            }
        }
        public long SetDistrict(long potamus_district_id, long region_id, string name, string poligon)
        {
            //long district_id = 0;
            SqlString sqlstr = new SqlString(poligon);
            SqlChars polychars = new SqlChars(sqlstr);
            using (var db = new DataModels.GeolocationDB())
            {
                object id = db.DictDistricts.InsertWithIdentity(() => new DataModels.DictDistricts
                {
                    Name = name,
                    Poligon = SqlGeography.STPolyFromText(polychars, 4326),
                    RegionId = region_id,
                    PotamusDistrictId = potamus_district_id
                });

                return Convert.ToInt64(id);
            }

            //    string cmd_text = @"
            //    INSERT INTO [dbo].[dict_districts]
            //               ([potamus_district_id]
            //               ,[region_id]
            //               ,[name]
            //               ,[poligon])
            //         VALUES
            //               (@potamus_district_id
            //               ,@region_id
            //               ,@name 
            //               ,geography::STPolyFromText('POLYGON((%poligon%))', 4326))
            //    SELECT scope_identity() id
            //    ";

            //cmd_text = cmd_text.Replace("%poligon%", poligon);
            //using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            //{
            //    SqlCommand cmd = connection.CreateCommand();
            //    cmd.CommandText = cmd_text;
            //    cmd.CommandType = CommandType.Text;

            //    cmd.Parameters.AddWithValue("@potamus_district_id", potamus_district_id);
            //    cmd.Parameters.AddWithValue("@region_id", region_id);
            //    cmd.Parameters.AddWithValue("@name", name);

            //    connection.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    DataTable tb = new DataTable();
            //    tb.Load(reader);
            //    reader.Close();
            //    connection.Close();

            //    district_id = Convert.ToInt64(tb.Rows[0]["id"]);
            //}

            //return district_id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="district_id"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void SetDistrictCoordinates(long district_id, decimal lat, decimal lng)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                db.DictDistrictsCoordinates.Insert(() => new DataModels.DictDistrictsCoordinates
                {
                    DistrictId = district_id,
                    Lat = lat,
                    Lng = lng
                });
            }

            //    string cmd_text = @"
            //    INSERT INTO [dbo].[dict_districts_coordinates]
            //               ([lat]
            //               ,[lng]
            //               ,[district_id])
            //         VALUES
            //               (@lat
            //               ,@lng
            //               ,@district_id)
            //    ";
            //using (SqlConnection connection = new SqlConnection(this.DbConnectString))
            //{
            //    SqlCommand cmd = connection.CreateCommand();
            //    cmd.CommandText = cmd_text;
            //    cmd.CommandType = CommandType.Text;

            //    cmd.Parameters.AddWithValue("@district_id", district_id);
            //    cmd.Parameters.AddWithValue("@lat", lat);
            //    cmd.Parameters.AddWithValue("@lng", lng);

            //    connection.Open();
            //    cmd.ExecuteNonQuery();
            //    connection.Close();
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataModels.DictRegions> GetRegions()
        {
            using (var db = new DataModels.GeolocationDB())
            {
                return db.DictRegions.ToList();
            }
            //List<Region> regions = new List<Region>();

            //string cmd_text = @"
            //    SELECT [region_id]
            //          ,[country_id]
            //          ,[name]
            //      FROM [dbo].[dict_regions]
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
            //        regions.Add(new Region {
            //            region_id = Convert.ToInt32(reader[0]),
            //            country_id = reader[1].ToString(),
            //            name = reader[2].ToString()
            //        });
            //    }
            //    connection.Close();
            //}
            //return regions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<DataModels.DictDistricts> GetDistrictsByRegionId(long id)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                return db.DictDistricts.Where(w => w.RegionId == id).ToList();
            }

            //List<District> districts = new List<District>();

            //string cmd_text = @"
            //            SELECT 
            //             [district_id]
            //                ,[potamus_district_id]
            //                ,[region_id]
            //                ,[name]
            //                ,[poligon].EnvelopeCenter().Long as lat
            //                ,[poligon].EnvelopeCenter().Lat  as lng
            //                ,is_centre_region
            //            FROM [dbo].[dict_districts] where [region_id] = {0}
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
            //        districts.Add(new District
            //        {
            //            district_id = Convert.ToInt32(reader[0]),
            //            potamus_district_id = Convert.ToInt32(reader[1]),
            //            region_id = Convert.ToInt32(reader[2]),
            //            name = reader[3].ToString(),
            //            lat = Convert.ToDecimal(reader[4]),
            //            lng = Convert.ToDecimal(reader[5]),
            //            is_region_center = Convert.ToBoolean(reader[6])
            //        });
            //    }
            //    connection.Close();
            //}
            //return districts;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataModels.DictDistricts GetDistrictById(long id)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                return db.DictDistricts.Where(w => w.DistrictId == id).FirstOrDefault();
            }

            //District district = new District();

            //string cmd_text = @"
            //            SELECT Top 1
            //             [district_id]
            //                ,[potamus_district_id]
            //                ,[region_id]
            //                ,[name]
            //                ,[poligon].EnvelopeCenter().Long as lat
            //                ,[poligon].EnvelopeCenter().Lat  as lng
            //            FROM [dbo].[dict_districts] where [district_id] = {0}
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
            //        district = new District
            //        {
            //            district_id = Convert.ToInt32(reader[0]),
            //            potamus_district_id = Convert.ToInt32(reader[1]),
            //            region_id = Convert.ToInt32(reader[2]),
            //            name = reader[3].ToString(),
            //            lat = Convert.ToDecimal(reader[4]),
            //            lng = Convert.ToDecimal(reader[5])
            //        };
            //    }
            //    connection.Close();
            //}
            //return district;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<DataModels.DictDistrictsCoordinates> GetDistrictCoordinates(long id)
        {
            using (var db = new DataModels.GeolocationDB())
            {
                return db.DictDistrictsCoordinates.Where(w => w.DistrictId == id).ToList();
            }

            //List<DistrictCoordinate> Coordinates = new List<DistrictCoordinate>();

            //string cmd_text = @"
            //        SELECT 
            //         [coordinate_id]
            //            ,[lat]
            //            ,[lng]
            //            ,[district_id]
            //        FROM [dbo].[dict_districts_coordinates] where district_id = {0}
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
            //        Coordinates.Add(new DistrictCoordinate
            //        {
            //            coordinate_id = Convert.ToInt32(reader[0]),
            //            lat = Convert.ToDecimal(reader[1]),
            //            lng = Convert.ToDecimal(reader[2]),
            //            district_id = Convert.ToInt64(reader[3])
            //        });
            //    }
            //    connection.Close();
            //}
            //return Coordinates;
        }

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
