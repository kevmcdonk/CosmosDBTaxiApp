using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "CosmosDBTaxiApp",
            collectionName: "CosmosDBTaxiAppDB",
            ConnectionStringSetting = "CosmosDBConnectionString",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> documents, TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                log.Verbose("Documents modified " + documents.Count);
                log.Verbose("First document Id " + documents[0].Id);
            }
        }
    }
}
