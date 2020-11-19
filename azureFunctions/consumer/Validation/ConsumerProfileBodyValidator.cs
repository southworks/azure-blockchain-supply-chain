using System;
using Newtonsoft.Json.Linq;

namespace SupplyChain.Consumer.Validation
{
	public static class ConsumerProfileBodyValidator
	{
		public static void ValidateBody(JObject body)
		{
			var userId = body.Value<string>("userId");

			if (String.IsNullOrWhiteSpace(userId))
			{
				throw new Exception("Missing parameter: userId cannot be null or empty.");
			}
		}
	}
}
