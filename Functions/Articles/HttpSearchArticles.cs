using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fmg.Models;
using Fmg.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fmg.Functions.Articles
{
    public class HttpSearchArticles
    {
        private readonly CosmosClient Cosmos;

        public HttpSearchArticles(CosmosClient cosmos)
        {
            Cosmos = cosmos;
        }

        [FunctionName(nameof(HttpSearchArticles))]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "articles")]
        HttpRequest request, ILogger logger, CancellationToken ct)
        {
            var agentId = request.Query["agentId"].FirstOrDefault();
            var category = request.Query["category"].FirstOrDefault();

            var json = await new StreamReader(request.Body).ReadToEndAsync();
            dynamic? body = JsonConvert.DeserializeObject(json);
            string? continuation = body?.continuation ?? null;

            var options = new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(agentId),
                MaxItemCount = 5
            };

            var container = Cosmos.GetContainer("Core", "Articles");

            var feed = container
                .GetItemLinqQueryable<Article>(false, continuation, options)
                .Where(entity => entity.AgentId == agentId)
                .Where(entity => entity.Category == category)
                .Where(entity => !entity.IsRemoved)
                .OrderByDescending(entity => entity.Created.Timestamp)
                .ToFeedIterator();

            if (!feed.HasMoreResults) return new OkObjectResult(new Page<Article>());

            var response = await feed.ReadNextAsync(ct);

            var page = new Page<Article>
            {
                Items = response,
                Continuation = response.ContinuationToken
            };

            return new OkObjectResult(page);
        }
    }
}