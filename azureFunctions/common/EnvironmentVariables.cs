using System;

namespace SupplyChain.Common
{
    public static class EnvironmentVariables
    {
        private static readonly string privateKey = Environment.GetEnvironmentVariable("PRIVATE_KEY");
        private static readonly string client = Environment.GetEnvironmentVariable("RPC_ENDPOINT");
        private static readonly string cosmosEndpointUri = Environment.GetEnvironmentVariable("COSMOS_DB_URI");
        private static readonly string cosmosPrimaryKey = Environment.GetEnvironmentVariable("COSMOS_DB_PRIMARY_KEY");
        private static readonly string cosmosDatabaseId = Environment.GetEnvironmentVariable("DATABASE_ID");
        private static readonly string userContainerId = Environment.GetEnvironmentVariable("USER_CONTAINER_ID");
        private static readonly string walletContainerId = Environment.GetEnvironmentVariable("WALLET_CONTAINER_ID");

        public static string PrivateKey => privateKey;
        public static string Client => client;
        public static string CosmosEndpointUri => cosmosEndpointUri;
        public static string CosmosPrimaryKey => cosmosPrimaryKey;
        public static string CosmosDatabaseId => cosmosDatabaseId;
        public static string UserContainerId => userContainerId;
        public static string WalletContainerId => walletContainerId;
    }
}
