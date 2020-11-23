using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataViewer.Models
{
    public class SamiService
    {
        private HttpClient httpClient;

        private readonly SamiConfig config;

        public SamiService(SamiConfig config, IHttpClientFactory httpClientFactory)
        {
            this.config = config;
            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(this.config.BaseUrl);
        }

        public async Task<List<SensorModel>> GetSensors(SearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/sensors/[your-key-here]
            string url = $"sensors/{model.Key}";
            string resultString;
            var result = await httpClient.GetAsync(url);
            List<SensorModel> sensors = null;
            if (result.IsSuccessStatusCode)
            {
                resultString = await result.Content.ReadAsStringAsync();
                sensors = JsonConvert.DeserializeObject<List<SensorModel>>(resultString);
                return sensors;
            }
            else
            {         
                Debug.WriteLine($"Error reading sensors: HTTP result was {result.StatusCode} - {result.ReasonPhrase}");
                return null;
            }
        }

        public async Task<List<SamiMeasurementModel>> GetMeasurements(SearchModel model)
        {
           
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            string url = $"measurements/{model.Key}";

            List<string> uriParams = new List<string>();
            if (false == string.IsNullOrEmpty(model.Obj))
            {
                uriParams.Add($"obj={model.Obj}");
            }
            if (false == string.IsNullOrEmpty(model.Tag))
            {
                uriParams.Add($"tag={model.Tag}");
            }
            if (model.Take.HasValue)
            {
                uriParams.Add($"take={model.Take.Value}");
            }
            if (false == string.IsNullOrEmpty(model.Sensors))
            {
                uriParams.Add($"data-tags={model.Sensors}");
            }
            if (model.From.HasValue)
            {
                uriParams.Add($"from={Uri.EscapeUriString(model.From.Value.ToString("yyyy-MM-ddTHH:mm:ss"))}"); 
            }
            if (model.To.HasValue)
            {
                uriParams.Add($"to={Uri.EscapeUriString(model.To.Value.ToString("yyyy-MM-ddTHH:mm:ss"))}");
            }

           
            if (uriParams.Count > 0)
            {
                url += $"?{string.Join("&", uriParams)}";
            }

            string resultString;
            var result = await httpClient.GetAsync(url);
            List<SamiMeasurementModel> samiData = null;
            if (result.IsSuccessStatusCode)
            {
                resultString = await result.Content.ReadAsStringAsync();
                samiData = JsonConvert.DeserializeObject<List<SamiMeasurementModel>>(resultString);
            }
            else
            {
               
                Debug.WriteLine($"Error reading measurements: HTTP result was {result.StatusCode} - {result.ReasonPhrase}");
            }
            return samiData;
        }
    }
}
