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
using SupplyChain.Common.Exceptions;
using SupplyChain.Common.Services;
using SupplyChain.Consumer.Validation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SupplyChain.Consumer
{
	public class CancelOrder
	{
		private readonly IUserProfileService _userProfileService;

		public CancelOrder(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("CancelOrder")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{

			try
			{
				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

				log.LogInformation($"Request body: {requestBody}");

				var requestBodyParsed = JObject.Parse(requestBody);

				ConsumerOperationsBodyValidator.ValidateBody(requestBodyParsed);

				var contractAddress = requestBodyParsed.Value<string>("contractAddress");

				var userId = requestBodyParsed.Value<string>("userId");

				var userProfile = await _userProfileService.GetUserProfile(userId);
				var keyStoreString = JsonConvert.SerializeObject(userProfile.UserKeyStore);

				var web3 = Web3Service.Initialize(keyStoreString, userProfile.Password);

				var retailerService = new RetailerService(web3, contractAddress);

				var cancelOrder = JsonConvert.DeserializeObject<CancelOrderFunction>(requestBody);

				await retailerService.CancelOrderRequestAsync(cancelOrder);

				log.LogInformation($"Order {cancelOrder.OrderId} canceled.");

				return new OkResult();
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in CancelOrder: {e.Message}");

				if (e is UserProfileException || e is SmartContractRevertException || e is RpcResponseException)
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
