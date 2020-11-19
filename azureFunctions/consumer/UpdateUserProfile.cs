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
	public class UpdateUserProfile
	{
		private static IUserProfileService _userProfileService;

		public UpdateUserProfile(IUserProfileService userProfileService)
		{
			_userProfileService = userProfileService;
		}

		[FunctionName("UpdateUserProfile")]
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
				var name = requestBodyParsed.Value<string>("name");
				var email = requestBodyParsed.Value<string>("email");

				var userProfile = await _userProfileService.GetUserProfile(userId);

				if (await _userProfileService.UpdateUserProfile(userId, userProfile, name, email))
				{
					return new OkObjectResult($"User {userId} updated successfully.");
				}
				else
				{
					return new OkObjectResult($"No changes were made to user {userId}'s profile.");
				}
			}
			catch (Exception e)
			{
				log.LogInformation($"The following Exception was raised in UpdateUserProfile: {e.Message}");

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
