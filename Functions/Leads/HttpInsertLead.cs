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

namespace Fmg.Functions.Leads
{
    public class HttpInsertLead
    {
        private readonly CosmosClient Cosmos;

        public HttpInsertLead(CosmosClient cosmos)
        {
            Cosmos = cosmos;
        }

        [FunctionName(nameof(HttpInsertLead))]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "leads")]
        HttpRequest request, ILogger logger, CancellationToken ct)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<Lead>(body);

            model.Updated = new Moment();

            var partitionKey = new PartitionKey(model.AgentId);

            var container = Cosmos.GetContainer("Core", "Leads");

            Lead entity = await container.UpsertItemAsync(model, partitionKey, null, ct);

            return new OkObjectResult(entity);
        }
    }
}