using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fmg.Models;
using Fmg.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fmg.Functions.Articles
{
    public class HttpInsertArticle
    {
        private readonly CosmosClient Cosmos;

        public HttpInsertArticle(CosmosClient cosmos)
        {
            Cosmos = cosmos;
        }

        [FunctionName(nameof(HttpInsertArticle))]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "articles")]
        HttpRequest request, ILogger logger, CancellationToken ct)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<Article>(body);

            model.Updated = new Moment();

            var partitionKey = new PartitionKey(model.AgentId);

            var container = Cosmos.GetContainer("Core", "Articles");

            Article entity = await container.UpsertItemAsync(model, partitionKey, null, ct);

            return new OkObjectResult(entity);
        }
    }
}