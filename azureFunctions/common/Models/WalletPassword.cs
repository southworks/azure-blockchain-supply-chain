using Newtonsoft.Json;

namespace SupplyChain.Common.Models
{
    public class WalletPassword
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("userProfileId")]
        public string UserProfileId { get; set; }
    }
}
