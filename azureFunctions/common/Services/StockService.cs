using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using SupplyChain.Common.ContractDefinition;
using System.Threading;
using System.Threading.Tasks;

namespace SupplyChain.Common.Services
{
    public partial class StockService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StockDeployment stockDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StockDeployment>().SendRequestAndWaitForReceiptAsync(stockDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StockDeployment stockDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StockDeployment>().SendRequestAsync(stockDeployment);
        }

        public static async Task<StockService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StockDeployment stockDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, stockDeployment, cancellationTokenSource);
            return new StockService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public StockService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddWhitelistedRequestAsync(AddWhitelistedFunction addWhitelistedFunction)
        {
            return ContractHandler.SendRequestAsync(addWhitelistedFunction);
        }

        public Task<TransactionReceipt> AddWhitelistedRequestAndWaitForReceiptAsync(AddWhitelistedFunction addWhitelistedFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistedFunction, cancellationToken);
        }

        public Task<string> AddWhitelistedRequestAsync(string account)
        {
            var addWhitelistedFunction = new AddWhitelistedFunction();
            addWhitelistedFunction.Account = account;

            return ContractHandler.SendRequestAsync(addWhitelistedFunction);
        }

        public Task<TransactionReceipt> AddWhitelistedRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var addWhitelistedFunction = new AddWhitelistedFunction();
            addWhitelistedFunction.Account = account;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistedFunction, cancellationToken);
        }

        public Task<string> RemoveWhitelistedRequestAsync(RemoveWhitelistedFunction removeWhitelistedFunction)
        {
            return ContractHandler.SendRequestAsync(removeWhitelistedFunction);
        }

        public Task<TransactionReceipt> RemoveWhitelistedRequestAndWaitForReceiptAsync(RemoveWhitelistedFunction removeWhitelistedFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(removeWhitelistedFunction, cancellationToken);
        }

        public Task<string> RemoveWhitelistedRequestAsync(string account)
        {
            var removeWhitelistedFunction = new RemoveWhitelistedFunction();
            removeWhitelistedFunction.Account = account;

            return ContractHandler.SendRequestAsync(removeWhitelistedFunction);
        }

        public Task<TransactionReceipt> RemoveWhitelistedRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var removeWhitelistedFunction = new RemoveWhitelistedFunction();
            removeWhitelistedFunction.Account = account;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(removeWhitelistedFunction, cancellationToken);
        }

        public Task<string> UpdateItemDescriptionRequestAsync(UpdateItemDescriptionFunction updateItemDescriptionFunction)
        {
            return ContractHandler.SendRequestAsync(updateItemDescriptionFunction);
        }

        public Task<TransactionReceipt> UpdateItemDescriptionRequestAndWaitForReceiptAsync(UpdateItemDescriptionFunction updateItemDescriptionFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateItemDescriptionFunction, cancellationToken);
        }

        public Task<string> UpdateItemDescriptionRequestAsync(uint id, string description)
        {
            var updateItemDescriptionFunction = new UpdateItemDescriptionFunction();
            updateItemDescriptionFunction.Id = id;
            updateItemDescriptionFunction.Description = description;

            return ContractHandler.SendRequestAsync(updateItemDescriptionFunction);
        }

        public Task<TransactionReceipt> UpdateItemDescriptionRequestAndWaitForReceiptAsync(uint id, string description, CancellationTokenSource cancellationToken = null)
        {
            var updateItemDescriptionFunction = new UpdateItemDescriptionFunction();
            updateItemDescriptionFunction.Id = id;
            updateItemDescriptionFunction.Description = description;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateItemDescriptionFunction, cancellationToken);
        }

        public Task<bool> IsWhitelistedQueryAsync(IsWhitelistedFunction isWhitelistedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsWhitelistedFunction, bool>(isWhitelistedFunction, blockParameter);
        }


        public Task<bool> IsWhitelistedQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isWhitelistedFunction = new IsWhitelistedFunction();
            isWhitelistedFunction.Account = account;

            return ContractHandler.QueryAsync<IsWhitelistedFunction, bool>(isWhitelistedFunction, blockParameter);
        }

        public Task<string> RenounceWhitelistAdminRequestAsync(RenounceWhitelistAdminFunction renounceWhitelistAdminFunction)
        {
            return ContractHandler.SendRequestAsync(renounceWhitelistAdminFunction);
        }

        public Task<string> RenounceWhitelistAdminRequestAsync()
        {
            return ContractHandler.SendRequestAsync<RenounceWhitelistAdminFunction>();
        }

        public Task<TransactionReceipt> RenounceWhitelistAdminRequestAndWaitForReceiptAsync(RenounceWhitelistAdminFunction renounceWhitelistAdminFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceWhitelistAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceWhitelistAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceWhitelistAdminFunction>(null, cancellationToken);
        }

        public Task<string> UpdateItemNameRequestAsync(UpdateItemNameFunction updateItemNameFunction)
        {
            return ContractHandler.SendRequestAsync(updateItemNameFunction);
        }

        public Task<TransactionReceipt> UpdateItemNameRequestAndWaitForReceiptAsync(UpdateItemNameFunction updateItemNameFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateItemNameFunction, cancellationToken);
        }

        public Task<string> UpdateItemNameRequestAsync(uint id, string name)
        {
            var updateItemNameFunction = new UpdateItemNameFunction();
            updateItemNameFunction.Id = id;
            updateItemNameFunction.Name = name;

            return ContractHandler.SendRequestAsync(updateItemNameFunction);
        }

        public Task<TransactionReceipt> UpdateItemNameRequestAndWaitForReceiptAsync(uint id, string name, CancellationTokenSource cancellationToken = null)
        {
            var updateItemNameFunction = new UpdateItemNameFunction();
            updateItemNameFunction.Id = id;
            updateItemNameFunction.Name = name;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateItemNameFunction, cancellationToken);
        }

        public Task<uint> GetItemStockQueryAsync(GetItemStockFunction getItemStockFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetItemStockFunction, uint>(getItemStockFunction, blockParameter);
        }


        public Task<uint> GetItemStockQueryAsync(uint id, BlockParameter blockParameter = null)
        {
            var getItemStockFunction = new GetItemStockFunction();
            getItemStockFunction.Id = id;

            return ContractHandler.QueryAsync<GetItemStockFunction, uint>(getItemStockFunction, blockParameter);
        }

        public Task<string> AddWhitelistAdminRequestAsync(AddWhitelistAdminFunction addWhitelistAdminFunction)
        {
            return ContractHandler.SendRequestAsync(addWhitelistAdminFunction);
        }

        public Task<TransactionReceipt> AddWhitelistAdminRequestAndWaitForReceiptAsync(AddWhitelistAdminFunction addWhitelistAdminFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistAdminFunction, cancellationToken);
        }

        public Task<string> AddWhitelistAdminRequestAsync(string account)
        {
            var addWhitelistAdminFunction = new AddWhitelistAdminFunction();
            addWhitelistAdminFunction.Account = account;

            return ContractHandler.SendRequestAsync(addWhitelistAdminFunction);
        }

        public Task<TransactionReceipt> AddWhitelistAdminRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var addWhitelistAdminFunction = new AddWhitelistAdminFunction();
            addWhitelistAdminFunction.Account = account;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistAdminFunction, cancellationToken);
        }

        public Task<GetItemOutputDTO> GetItemQueryAsync(GetItemFunction getItemFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetItemFunction, GetItemOutputDTO>(getItemFunction, blockParameter);
        }

        public Task<GetItemOutputDTO> GetItemQueryAsync(uint id, BlockParameter blockParameter = null)
        {
            var getItemFunction = new GetItemFunction();
            getItemFunction.Id = id;

            return ContractHandler.QueryDeserializingToObjectAsync<GetItemFunction, GetItemOutputDTO>(getItemFunction, blockParameter);
        }

        public Task<string> DecreaseItemStockRequestAsync(DecreaseItemStockFunction decreaseItemStockFunction)
        {
            return ContractHandler.SendRequestAsync(decreaseItemStockFunction);
        }

        public Task<TransactionReceipt> DecreaseItemStockRequestAndWaitForReceiptAsync(DecreaseItemStockFunction decreaseItemStockFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseItemStockFunction, cancellationToken);
        }

        public Task<string> DecreaseItemStockRequestAsync(uint id, uint amount)
        {
            var decreaseItemStockFunction = new DecreaseItemStockFunction();
            decreaseItemStockFunction.Id = id;
            decreaseItemStockFunction.Amount = amount;

            return ContractHandler.SendRequestAsync(decreaseItemStockFunction);
        }

        public Task<TransactionReceipt> DecreaseItemStockRequestAndWaitForReceiptAsync(uint id, uint amount, CancellationTokenSource cancellationToken = null)
        {
            var decreaseItemStockFunction = new DecreaseItemStockFunction();
            decreaseItemStockFunction.Id = id;
            decreaseItemStockFunction.Amount = amount;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseItemStockFunction, cancellationToken);
        }

        public Task<bool> IsWhitelistAdminQueryAsync(IsWhitelistAdminFunction isWhitelistAdminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsWhitelistAdminFunction, bool>(isWhitelistAdminFunction, blockParameter);
        }


        public Task<bool> IsWhitelistAdminQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isWhitelistAdminFunction = new IsWhitelistAdminFunction();
            isWhitelistAdminFunction.Account = account;

            return ContractHandler.QueryAsync<IsWhitelistAdminFunction, bool>(isWhitelistAdminFunction, blockParameter);
        }

        public Task<string> CreateItemRequestAsync(CreateItemFunction createItemFunction)
        {
            return ContractHandler.SendRequestAsync(createItemFunction);
        }

        public Task<TransactionReceipt> CreateItemRequestAndWaitForReceiptAsync(CreateItemFunction createItemFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(createItemFunction, cancellationToken);
        }

        public Task<string> CreateItemRequestAsync(string name, string description, string category, uint stock)
        {
            var createItemFunction = new CreateItemFunction();
            createItemFunction.Name = name;
            createItemFunction.Description = description;
            createItemFunction.Category = category;
            createItemFunction.Stock = stock;

            return ContractHandler.SendRequestAsync(createItemFunction);
        }

        public Task<TransactionReceipt> CreateItemRequestAndWaitForReceiptAsync(string name, string description, string category, uint stock, CancellationTokenSource cancellationToken = null)
        {
            var createItemFunction = new CreateItemFunction();
            createItemFunction.Name = name;
            createItemFunction.Description = description;
            createItemFunction.Category = category;
            createItemFunction.Stock = stock;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(createItemFunction, cancellationToken);
        }

        public Task<string> RenounceWhitelistedRequestAsync(RenounceWhitelistedFunction renounceWhitelistedFunction)
        {
            return ContractHandler.SendRequestAsync(renounceWhitelistedFunction);
        }

        public Task<string> RenounceWhitelistedRequestAsync()
        {
            return ContractHandler.SendRequestAsync<RenounceWhitelistedFunction>();
        }

        public Task<TransactionReceipt> RenounceWhitelistedRequestAndWaitForReceiptAsync(RenounceWhitelistedFunction renounceWhitelistedFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceWhitelistedFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceWhitelistedRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceWhitelistedFunction>(null, cancellationToken);
        }

        public Task<string> RemoveItemRequestAsync(RemoveItemFunction removeItemFunction)
        {
            return ContractHandler.SendRequestAsync(removeItemFunction);
        }

        public Task<TransactionReceipt> RemoveItemRequestAndWaitForReceiptAsync(RemoveItemFunction removeItemFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(removeItemFunction, cancellationToken);
        }

        public Task<string> RemoveItemRequestAsync(uint id)
        {
            var removeItemFunction = new RemoveItemFunction();
            removeItemFunction.Id = id;

            return ContractHandler.SendRequestAsync(removeItemFunction);
        }

        public Task<TransactionReceipt> RemoveItemRequestAndWaitForReceiptAsync(uint id, CancellationTokenSource cancellationToken = null)
        {
            var removeItemFunction = new RemoveItemFunction();
            removeItemFunction.Id = id;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(removeItemFunction, cancellationToken);
        }

        public Task<string> IncreaseItemStockRequestAsync(IncreaseItemStockFunction increaseItemStockFunction)
        {
            return ContractHandler.SendRequestAsync(increaseItemStockFunction);
        }

        public Task<TransactionReceipt> IncreaseItemStockRequestAndWaitForReceiptAsync(IncreaseItemStockFunction increaseItemStockFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseItemStockFunction, cancellationToken);
        }

        public Task<string> IncreaseItemStockRequestAsync(uint id, uint amount)
        {
            var increaseItemStockFunction = new IncreaseItemStockFunction();
            increaseItemStockFunction.Id = id;
            increaseItemStockFunction.Amount = amount;

            return ContractHandler.SendRequestAsync(increaseItemStockFunction);
        }

        public Task<TransactionReceipt> IncreaseItemStockRequestAndWaitForReceiptAsync(uint id, uint amount, CancellationTokenSource cancellationToken = null)
        {
            var increaseItemStockFunction = new IncreaseItemStockFunction();
            increaseItemStockFunction.Id = id;
            increaseItemStockFunction.Amount = amount;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseItemStockFunction, cancellationToken);
        }

        public Task<string> UpdateItemCategoryRequestAsync(UpdateItemCategoryFunction updateItemCategoryFunction)
        {
            return ContractHandler.SendRequestAsync(updateItemCategoryFunction);
        }

        public Task<TransactionReceipt> UpdateItemCategoryRequestAndWaitForReceiptAsync(UpdateItemCategoryFunction updateItemCategoryFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateItemCategoryFunction, cancellationToken);
        }

        public Task<string> UpdateItemCategoryRequestAsync(uint id, string category)
        {
            var updateItemCategoryFunction = new UpdateItemCategoryFunction();
            updateItemCategoryFunction.Id = id;
            updateItemCategoryFunction.Category = category;

            return ContractHandler.SendRequestAsync(updateItemCategoryFunction);
        }

        public Task<TransactionReceipt> UpdateItemCategoryRequestAndWaitForReceiptAsync(uint id, string category, CancellationTokenSource cancellationToken = null)
        {
            var updateItemCategoryFunction = new UpdateItemCategoryFunction();
            updateItemCategoryFunction.Id = id;
            updateItemCategoryFunction.Category = category;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateItemCategoryFunction, cancellationToken);
        }
    }
}
