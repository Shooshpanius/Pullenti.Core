using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pullenti.Morph;
using Pullenti.Ner;
using Pullenti.Semantic;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace PullentiAPI.Controllers
{

    public class RequestData
    {
        public required string text { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class Ner() : ControllerBase
    {



        [HttpPost("getNer")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]

        public string getNerPOST([FromBody] RequestData requestData)
        {
            Task<string> nerAsync = getNerAsync(requestData.text);
            return nerAsync.Result;
        }





        private async Task<string> getNerAsync(string text)
        {
            Processor processor = ProcessorService.CreateProcessor();

            List<MorphToken> nerResult = MorphologyService.Process(text);

            string nerJsonResult = JsonConvert.SerializeObject(nerResult);
            return nerJsonResult;

        }

    }

}
