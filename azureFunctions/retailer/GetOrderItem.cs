using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupplyChain.Common;
using SupplyChain.Common.Constants;
using SupplyChain.Common.ContractDefinition;
using SupplyChain.Common.Services;
using SupplyChain.Common.Validation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SupplyChain.Retailer
{
	public class GetOrderItem
	{
		private readonly IUserProfileService _userProfileService;

		public GetOrderItem(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("GetOrderItem")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			try
			{
				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

				log.LogInformation($"Request body: {requestBody}");

				var requestBodyParsed = JObject.Parse(requestBody);

				BaseBodyValidator.ValidateBody(requestBodyParsed);

				var contractAddress = requestBodyParsed.Value<string>("contractAddress");

				var web3 = Web3Service.Initialize();

				var retailerService = new RetailerService(web3, contractAddress);

				var getOrderItem = JsonConvert.DeserializeObject<GetOrderItemFunction>(requestBody);

				var orderItem = await retailerService.GetOrderItemQueryAsync(getOrderItem);

				log.LogInformation($"Order item id: {orderItem.Id}");

				return new OkObjectResult(orderItem);
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in GetOrderItem: {e.Message}");

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
