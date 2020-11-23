using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DataViewer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataViewer.Controllers
{
    [Produces("application/json")]
    [Route("api/Data")]
    public class DataController : Controller
    {
        private SamiService samiService;

        public DataController(SamiService samiService)
        {
            this.samiService = samiService;
        }

        // GET: api/Data
        [HttpGet]
        public async Task<List<SamiMeasurementModel>> Get()
        {
            // haetaan hakuehdot sessiosta
            string jsondata = HttpContext.Session.GetString("search-model");
            SearchModel searchModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchModel>(jsondata);

            var samiData = await samiService.GetMeasurements(searchModel);
            return samiData;
        }
    }
}
