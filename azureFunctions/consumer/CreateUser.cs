using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SupplyChain.Common.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SupplyChain.Consumer
{
    public class CreateUser
    {
        private static IUserProfileService _userProfileService;

        public CreateUser(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [FunctionName("CreateUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var requestBodyParsed = JObject.Parse(requestBody);

                var name = requestBodyParsed.Value<string>("name");
                var email = requestBodyParsed.Value<string>("email");

                var userId = await _userProfileService.CreateAndUploadUserProfile(name, email);

                return new OkObjectResult(userId);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
