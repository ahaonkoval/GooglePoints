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
    public class DistrictController : ApiController
    {
        // GET api/district
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/district/5
        public DataModels.DictDistricts GetDistrictById(int id)
        {
            using (DictGeoData dict = new DictGeoData())
                return dict.GetDistrictById(id);
            //DictEpicetnrK dict = new DictEpicetnrK();
            //return dict.GetDistrictById(id);
        }

        // POST api/district
        public void Post([FromBody]string value)
        {
        }

        // PUT api/district/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/district/5
        public void Delete(int id)
        {
        }
        public IEnumerable<DataModels.DictDistricts> GetDistricts(int id)
        {
            using (DictGeoData dict = new DictGeoData())
                return dict.GetDistrictsByRegionId(id);

            //DictEpicetnrK dict = new DictEpicetnrK();
            //return dict.GetDistrictsByRegionId(id);
        }
        
    }
}
