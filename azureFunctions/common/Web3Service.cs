using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;

namespace SupplyChain.Common
{
    public static class Web3Service
    {
        public static Web3 Initialize(string userKeyStore = null, string password = null)
        {
            try
            {
                Account account;

                if (userKeyStore != null && password != null)
                {
                    account = Account.LoadFromKeyStore(userKeyStore, password);
                }
                else
                {
                    account = new Account(EnvironmentVariables.PrivateKey);
                }

                var web3 = new Web3(account, new RpcClient(new Uri(EnvironmentVariables.Client)));

                return web3;
            }
            catch (Exception)
            {
                throw new Exception("Failed to connect to Ethereum network: invalid credentials.");
            }
        }
    }
}
