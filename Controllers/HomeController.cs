using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataViewer.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace DataViewer.Controllers
{
    public class HomeController : Controller
    {
        private SamiService samiService;

        public HomeController(SamiService samiService)
        {
            this.samiService = samiService;
        }

        [Authorize]
        public IActionResult Index()
        {
            SearchModel searchModel = null;
            string jsondata = HttpContext.Session.GetString("search-model");
            if (false == string.IsNullOrEmpty(jsondata))
            {
                searchModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchModel>(jsondata);
            }
            else
            {
                searchModel = new SearchModel()
                {
                    From = new DateTime(2019, 11, 11)
                };
            }

            return View(searchModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchModel model)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString("search-model", json);
            var sensors = await samiService.GetSensors(model);
            return View(new SensorSelectViewModel()
            {
                SearchModel = model,
                Sensors = sensors
            });
        }

        [HttpPost]
        public IActionResult SearchData(SearchModel model)
        {
            string jsondata = HttpContext.Session.GetString("search-model");
            SearchModel searchModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchModel>(jsondata);
            searchModel.From = model.From;
            searchModel.To = model.From.HasValue ? model.From.Value.AddDays(1) : default(DateTime?);
            searchModel.Sensors = model.Sensors;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(searchModel);
            HttpContext.Session.SetString("search-model", json);

            return View("Graph");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
