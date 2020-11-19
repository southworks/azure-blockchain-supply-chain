using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SupplyChain.Common.DataAccess;
using SupplyChain.Common.Services;
using SupplyChain.Retailer;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SupplyChain.Retailer
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();

            builder.Services.AddLogging();
        }
    }
}
