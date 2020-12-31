using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fmg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Fmg.Functions.Agents
{
    public class HttpFindAgent
    {
        private readonly CosmosClient Cosmos;

        public HttpFindAgent(CosmosClient cosmos)
        {
            Cosmos = cosmos;
        }

        [FunctionName(nameof(HttpFindAgent))]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "agents")]
        HttpRequest request, ILogger logger, CancellationToken ct)
        {
            var id = request.Query["id"].FirstOrDefault();

            var options = new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(id)
            };

            var container = Cosmos.GetContainer("Core", "Agents");

            var feed = container
                .GetItemLinqQueryable<Agent>(false, null, options)
                .Where(entity => entity.Id == id)
                .Where(entity => !entity.IsRemoved)
                .ToFeedIterator();

            var entities = new List<Agent>();

            while (feed.HasMoreResults)
            {
                entities.AddRange(await feed.ReadNextAsync(ct));
            }

            var entity = entities.FirstOrDefault();

            return new OkObjectResult(entity);
        }
    }
}