using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using SupplyChain.Common;
using SupplyChain.Common.Constants;
using SupplyChain.Common.ContractDefinition;
using SupplyChain.Common.Services;
using SupplyChain.Common.Validation;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.Stock
{
    public class CreateItem
    {
        private static IUserProfileService _userProfileService;

        public CreateItem(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [FunctionName("CreateItem")]
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

                var stockService = new StockService(web3, contractAddress);

                var createItem = JsonConvert.DeserializeObject<CreateItemFunction>(requestBody);

                var receiptForCreateItemFunctionCall = await stockService.CreateItemRequestAndWaitForReceiptAsync(createItem);

                var itemCreatedEvent = receiptForCreateItemFunctionCall.DecodeAllEvents<ItemCreatedEventDTO>().FirstOrDefault().Event;

                log.LogInformation("Event:");
                log.LogInformation($" Message: {itemCreatedEvent.Message}");
                log.LogInformation($" Item ID: {itemCreatedEvent.Id}");

				return new OkObjectResult(itemCreatedEvent.Id);
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in CreateItem: {e.Message}");

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
