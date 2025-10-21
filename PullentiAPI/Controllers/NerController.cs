using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pullenti.Morph;
using Pullenti.Ner;
using Pullenti.Semantic;
using System.Text.Json;

namespace PullentiAPI.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class Ner() : ControllerBase
    {


        [HttpPost("getNer")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]

        public string getNerPOST([FromBody] string text)
        {
            Task<string> nerAsync = getNerAsync(text);
            return nerAsync.Result;
        }





        private async Task<string> getNerAsync(string text)
        {
            Pullenti.Sdk.InitializeAll();


            // создаём экземпляр процессора со стандартными анализаторами
            Processor processor = ProcessorService.CreateProcessor();


            //MorpholodyService
            // запускаем на тексте text
            //AnalysisResult result = processor.Process(new SourceOfAnalysis("Здесь Вы можете добавлять ссылки быстрого доступа"));

            List<MorphToken> nerResult = MorphologyService.Process("Здесь Вы можете добавлять ссылки быстрого доступа");

            string nerJsonResult = JsonConvert.SerializeObject(nerResult);
            return nerJsonResult;

            //// получили выделенные сущности
            //foreach (Referent entity in result.Entities)
            //{
            //    Console.WriteLine(entity.ToString());
            //}

            //List<Referent> nerResult = result.Entities;
            //string nerJsonResult = JsonSerializer.Serialize(nerResult);
            //return nerJsonResult;

        }





    }







}
