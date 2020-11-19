using Nethereum.KeyStore.Model;
using Newtonsoft.Json;

namespace SupplyChain.Common.Models
{
    public class UserProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userKeyStore")]
        public KeyStore<ScryptParams> UserKeyStore { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
