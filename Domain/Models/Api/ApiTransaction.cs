using Domain.Enums;
using Newtonsoft.Json;

namespace Domain.Models.Api
{
    public class ApiTransaction
    {
        [JsonProperty("id")]
        public string TransactionId { get; set; }

        [JsonProperty("payment")]
        public string Payment { get; set; }

        public TransactionStatusEnum Status { get; set; }
    }
}
