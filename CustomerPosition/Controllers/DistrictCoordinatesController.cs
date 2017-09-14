using Core;
using CoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Core.Objects;

namespace CustomerPosition.Controllers
{
    public class DistrictCoordinatesController : ApiController
    {
        // GET api/districtcoordinates
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/districtcoordinates/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/districtcoordinates
        public void Post([FromBody]string value)
        {
        }

        // PUT api/districtcoordinates/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/districtcoordinates/5
        public void Delete(int id)
        {
        }
        public IEnumerable<DataModels.DictDistrictsCoordinates> GetDistrictCoordinates(int id)
        {
            using (DictGeoData dict = new DictGeoData())
                return dict.GetDistrictCoordinates(id);
            //DictEpicetnrK dict = new DictEpicetnrK();
            //return dict.GetDistrictCoordinates(id);
        }
    }
}
