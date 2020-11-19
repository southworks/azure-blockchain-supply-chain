using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts;
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
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.Consumer
{
	public class CreateOrder
	{
		private readonly IUserProfileService _userProfileService;

		public CreateOrder(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("CreateOrder")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			try
			{
				log.LogInformation("C# HTTP trigger function processed a request.");

				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

				var requestBodyParsed = JObject.Parse(requestBody);

				ConsumerOperationsBodyValidator.ValidateBody(requestBodyParsed);

				var contractAddress = requestBodyParsed.Value<string>("contractAddress");

				var userId = requestBodyParsed.Value<string>("userId");

				var userProfile = await _userProfileService.GetUserProfile(userId);
				var keyStoreString = JsonConvert.SerializeObject(userProfile.UserKeyStore);

				var web3 = Web3Service.Initialize(keyStoreString, userProfile.Password);

				var retailerService = new RetailerService(web3, contractAddress);

				var createOrder = JsonConvert.DeserializeObject<CreateOrderFunction>(requestBody);
				createOrder.ClientEmail = userProfile.Email;

				var receiptForCreateOrderFunctionCall = await retailerService.CreateOrderRequestAndWaitForReceiptAsync(createOrder);

				var orderCreatedEvent = receiptForCreateOrderFunctionCall.DecodeAllEvents<OrderCreatedEventDTO>().FirstOrDefault().Event;

				log.LogInformation("Event:");
				log.LogInformation($" Message: {orderCreatedEvent.Message}");
				log.LogInformation($" Order ID: {orderCreatedEvent.OrderId}");
				log.LogInformation($" Status: {orderCreatedEvent.Status}");

				log.LogInformation($"Finished storing an int: Tx Hash: {receiptForCreateOrderFunctionCall.TransactionHash}");
				log.LogInformation($"Finished storing an int: Tx Status: {receiptForCreateOrderFunctionCall.Status.Value}");

				return new OkObjectResult(orderCreatedEvent.OrderId);
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in CreateOrder: {e.Message}");

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
