using System.IO;
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

namespace Fmg.Functions.Agents
{
    public class HttpInsertAgent
    {
        private readonly CosmosClient Cosmos;

        public HttpInsertAgent(CosmosClient cosmos)
        {
            Cosmos = cosmos;
        }

        [FunctionName(nameof(HttpInsertAgent))]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "agents")]
        HttpRequest request, ILogger logger, CancellationToken ct)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<Agent>(body);

            model.Updated = new Moment();

            var partitionKey = new PartitionKey(model.Id);

            var container = Cosmos.GetContainer("Core", "Agents");

            Agent entity = await container.UpsertItemAsync(model, partitionKey, null, ct);

            return new OkObjectResult(entity);
        }
    }
}