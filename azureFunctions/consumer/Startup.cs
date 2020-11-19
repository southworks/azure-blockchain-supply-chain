using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.KeyStore;
using Nethereum.KeyStore.Model;
using SupplyChain.Common.DataAccess;
using SupplyChain.Common.Services;
using SupplyChain.Consumer;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SupplyChain.Consumer
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IKeyStoreService<ScryptParams>, KeyStoreScryptService>(x => { return new KeyStoreScryptService(); });
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            builder.Services.AddScoped<IWalletPasswordRepository, WalletPasswordRepository>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();

            builder.Services.AddLogging();
        }
    }
}
