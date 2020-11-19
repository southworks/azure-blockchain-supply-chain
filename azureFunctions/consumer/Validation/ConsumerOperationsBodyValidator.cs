using System;
using SupplyChain.Common.Validation;
using Newtonsoft.Json.Linq;

namespace SupplyChain.Consumer.Validation
{
	public static class ConsumerOperationsBodyValidator
	{
		public static void ValidateBody(JObject body)
		{
			BaseBodyValidator.ValidateBody(body);

			var userId = body.Value<string>("userId");

			if (String.IsNullOrWhiteSpace(userId))
			{
				throw new Exception("Missing parameter: userId cannot be null or empty.");
			}
		}
	}
}
