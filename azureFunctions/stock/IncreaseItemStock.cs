using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupplyChain.Common;
using SupplyChain.Common.Constants;
using SupplyChain.Common.ContractDefinition;
using SupplyChain.Common.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SupplyChain.Stock
{
	public class IncreaseItemStock
	{
		private static IUserProfileService _userProfileService;

		public IncreaseItemStock(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("IncreaseItemStock")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			try
			{
				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

				log.LogInformation($"Request body: {requestBody}");

				var requestBodyParsed = JObject.Parse(requestBody);
				var contractAddress = requestBodyParsed.Value<string>("contractAddress");

				var web3 = Web3Service.Initialize();

				var stockService = new StockService(web3, contractAddress);

				var increaseItemStock = JsonConvert.DeserializeObject<IncreaseItemStockFunction>(requestBody);

				await stockService.IncreaseItemStockRequestAsync(increaseItemStock);

				log.LogInformation($"Item stock increased by {increaseItemStock.Amount}");

				return new OkResult();
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in IncreaseItemStock: {e.Message}");

				if (e is SmartContractRevertException || e is RpcResponseException)
				{
					return new ContentResult
					{
						StatusCode = 401,
						Content = Messages.UnAuthorizedMessage
					};
				}

				return new BadRequestObjectResult(e.Message);
			}
		}
	}
}
