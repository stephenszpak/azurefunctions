using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunctionsLab
{
    public static class Add
    {
        public static int Add2(
            HttpRequestMessage req,
            int x,
            int y,
            TraceWriter log)
        {
            return x + y;
        }

        // now when you run you will add
        // -- http://localhost:7071/api/Adder/3/5 --

        [FunctionName("Process")]
        [return: Table("Results")]
        public static TableRow Process(
            [HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "Process/{x:int}/{y:int}")]
            HttpRequestMessage req,
            int x,
            int y,
            [Table("Results", "sums", "{x}_{y}")]
            TableRow tableRow,
            TraceWriter log)
        {

            if (tableRow != null)
            {
                log.Info($"{x} + {y} already exists");
                return null;
            }
            log.Info($"Processing {x} + {y}");

            return new TableRow()
            {
                PartitionKey = "sums",
                RowKey = $"{x}_{y}",
                X = x,
                Y = y,
                Sum = x + y
            };
        }
    }

    public class TableRow : TableEntity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Sum { get; set; }
    }
}
