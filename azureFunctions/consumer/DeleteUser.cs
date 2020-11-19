using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SupplyChain.Common.Constants;
using SupplyChain.Common.Exceptions;
using SupplyChain.Common.Services;
using SupplyChain.Consumer.Validation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SupplyChain.Consumer
{
	public class DeleteUser
	{
		private static IUserProfileService _userProfileService;

		public DeleteUser(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("DeleteUser")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			try
			{
				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
				var requestBodyParsed = JObject.Parse(requestBody);

				ConsumerProfileBodyValidator.ValidateBody(requestBodyParsed);

				var userId = requestBodyParsed.Value<string>("userId");

				await _userProfileService.DeleteUserProfile(userId);

				return new OkObjectResult($"User {userId} deleted");
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in DeleteUser: {e.Message}");

				if (e is UserProfileException)
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
