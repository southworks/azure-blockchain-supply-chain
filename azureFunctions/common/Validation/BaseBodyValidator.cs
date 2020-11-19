using System;
using Newtonsoft.Json.Linq;

namespace SupplyChain.Common.Validation
{
	public static class BaseBodyValidator
	{
		public static void ValidateBody(JObject body)
		{
			var contractAddress = body.Value<string>("contractAddress");

			if (String.IsNullOrWhiteSpace(contractAddress))
			{
				throw new Exception("Missing parameter: contractAddress cannot be null or empty.");
			}
		}
	}
}
