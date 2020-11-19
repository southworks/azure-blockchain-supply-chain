using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;
using SupplyChain.Common;
using SupplyChain.Common.Constants;
using SupplyChain.Common.Services;
using SupplyChain.Common.Validation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SupplyChain.Retailer
{
	public class GetNewOrders
	{
		private readonly IUserProfileService _userProfileService;

		public GetNewOrders(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("GetNewOrders")]
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

				var newOrders = await retailerService.GetNewOrdersQueryAsync();

				log.LogInformation($"The amount of new orders is: {newOrders.Count}");

				return new OkObjectResult(newOrders);
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in GetNewOrders: {e.Message}");

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
