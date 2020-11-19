using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using SupplyChain.Common.ContractDefinition;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SupplyChain.Common.Services
{
    public partial class RetailerService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, RetailerDeployment retailerDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<RetailerDeployment>().SendRequestAndWaitForReceiptAsync(retailerDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, RetailerDeployment retailerDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<RetailerDeployment>().SendRequestAsync(retailerDeployment);
        }

        public static async Task<RetailerService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, RetailerDeployment retailerDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, retailerDeployment, cancellationTokenSource);
            return new RetailerService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public RetailerService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> RejectOrderRequestAsync(RejectOrderFunction rejectOrderFunction)
        {
            return ContractHandler.SendRequestAsync(rejectOrderFunction);
        }

        public Task<TransactionReceipt> RejectOrderRequestAndWaitForReceiptAsync(RejectOrderFunction rejectOrderFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(rejectOrderFunction, cancellationToken);
        }

        public Task<string> RejectOrderRequestAsync(uint orderId)
        {
            var rejectOrderFunction = new RejectOrderFunction();
            rejectOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAsync(rejectOrderFunction);
        }

        public Task<TransactionReceipt> RejectOrderRequestAndWaitForReceiptAsync(uint orderId, CancellationTokenSource cancellationToken = null)
        {
            var rejectOrderFunction = new RejectOrderFunction();
            rejectOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(rejectOrderFunction, cancellationToken);
        }

        public Task<GetOrderItemOutputDTO> GetOrderItemQueryAsync(GetOrderItemFunction getOrderItemFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetOrderItemFunction, GetOrderItemOutputDTO>(getOrderItemFunction, blockParameter);
        }

        public Task<GetOrderItemOutputDTO> GetOrderItemQueryAsync(uint orderId, uint itemId, BlockParameter blockParameter = null)
        {
            var getOrderItemFunction = new GetOrderItemFunction();
            getOrderItemFunction.OrderId = orderId;
            getOrderItemFunction.ItemId = itemId;

            return ContractHandler.QueryDeserializingToObjectAsync<GetOrderItemFunction, GetOrderItemOutputDTO>(getOrderItemFunction, blockParameter);
        }

        public Task<string> CancelOrderRequestAsync(CancelOrderFunction cancelOrderFunction)
        {
            return ContractHandler.SendRequestAsync(cancelOrderFunction);
        }

        public Task<TransactionReceipt> CancelOrderRequestAndWaitForReceiptAsync(CancelOrderFunction cancelOrderFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelOrderFunction, cancellationToken);
        }

        public Task<string> CancelOrderRequestAsync(uint orderId)
        {
            var cancelOrderFunction = new CancelOrderFunction();
            cancelOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAsync(cancelOrderFunction);
        }

        public Task<TransactionReceipt> CancelOrderRequestAndWaitForReceiptAsync(uint orderId, CancellationTokenSource cancellationToken = null)
        {
            var cancelOrderFunction = new CancelOrderFunction();
            cancelOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(cancelOrderFunction, cancellationToken);
        }

        public Task<string> UpdateMinItemsPerOrderRequestAsync(UpdateMinItemsPerOrderFunction updateMinItemsPerOrderFunction)
        {
            return ContractHandler.SendRequestAsync(updateMinItemsPerOrderFunction);
        }

        public Task<TransactionReceipt> UpdateMinItemsPerOrderRequestAndWaitForReceiptAsync(UpdateMinItemsPerOrderFunction updateMinItemsPerOrderFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateMinItemsPerOrderFunction, cancellationToken);
        }

        public Task<string> UpdateMinItemsPerOrderRequestAsync(uint minItems)
        {
            var updateMinItemsPerOrderFunction = new UpdateMinItemsPerOrderFunction();
            updateMinItemsPerOrderFunction.MinItems = minItems;

            return ContractHandler.SendRequestAsync(updateMinItemsPerOrderFunction);
        }

        public Task<TransactionReceipt> UpdateMinItemsPerOrderRequestAndWaitForReceiptAsync(uint minItems, CancellationTokenSource cancellationToken = null)
        {
            var updateMinItemsPerOrderFunction = new UpdateMinItemsPerOrderFunction();
            updateMinItemsPerOrderFunction.MinItems = minItems;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateMinItemsPerOrderFunction, cancellationToken);
        }

        public Task<string> CreateOrderRequestAsync(CreateOrderFunction createOrderFunction)
        {
            return ContractHandler.SendRequestAsync(createOrderFunction);
        }

        public Task<TransactionReceipt> CreateOrderRequestAndWaitForReceiptAsync(CreateOrderFunction createOrderFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(createOrderFunction, cancellationToken);
        }

        public Task<string> CreateOrderRequestAsync(List<uint> itemIds, List<uint> itemQuantities, string clientEmail)
        {
            var createOrderFunction = new CreateOrderFunction();
            createOrderFunction.ItemIds = itemIds;
            createOrderFunction.ItemQuantities = itemQuantities;
            createOrderFunction.ClientEmail = clientEmail;

            return ContractHandler.SendRequestAsync(createOrderFunction);
        }

        public Task<TransactionReceipt> CreateOrderRequestAndWaitForReceiptAsync(List<uint> itemIds, List<uint> itemQuantities, string clientEmail, CancellationTokenSource cancellationToken = null)
        {
            var createOrderFunction = new CreateOrderFunction();
            createOrderFunction.ItemIds = itemIds;
            createOrderFunction.ItemQuantities = itemQuantities;
            createOrderFunction.ClientEmail = clientEmail;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(createOrderFunction, cancellationToken);
        }

        public Task<bool> CheckOrderItemsAvailabilityQueryAsync(CheckOrderItemsAvailabilityFunction checkOrderItemsAvailabilityFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CheckOrderItemsAvailabilityFunction, bool>(checkOrderItemsAvailabilityFunction, blockParameter);
        }


        public Task<bool> CheckOrderItemsAvailabilityQueryAsync(uint orderId, BlockParameter blockParameter = null)
        {
            var checkOrderItemsAvailabilityFunction = new CheckOrderItemsAvailabilityFunction();
            checkOrderItemsAvailabilityFunction.OrderId = orderId;

            return ContractHandler.QueryAsync<CheckOrderItemsAvailabilityFunction, bool>(checkOrderItemsAvailabilityFunction, blockParameter);
        }

        public Task<OrdersOutputDTO> OrdersQueryAsync(OrdersFunction ordersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<OrdersFunction, OrdersOutputDTO>(ordersFunction, blockParameter);
        }

        public Task<OrdersOutputDTO> OrdersQueryAsync(uint returnValue1, BlockParameter blockParameter = null)
        {
            var ordersFunction = new OrdersFunction();
            ordersFunction.ReturnValue1 = returnValue1;

            return ContractHandler.QueryDeserializingToObjectAsync<OrdersFunction, OrdersOutputDTO>(ordersFunction, blockParameter);
        }

        public Task<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
            return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public Task<string> RenounceOwnershipRequestAsync()
        {
            return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }


        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<bool> IsOwnerQueryAsync(IsOwnerFunction isOwnerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsOwnerFunction, bool>(isOwnerFunction, blockParameter);
        }


        public Task<bool> IsOwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsOwnerFunction, bool>(null, blockParameter);
        }

        public Task<string> UpdateStockContractRequestAsync(UpdateStockContractFunction updateStockContractFunction)
        {
            return ContractHandler.SendRequestAsync(updateStockContractFunction);
        }

        public Task<TransactionReceipt> UpdateStockContractRequestAndWaitForReceiptAsync(UpdateStockContractFunction updateStockContractFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateStockContractFunction, cancellationToken);
        }

        public Task<string> UpdateStockContractRequestAsync(string newContract)
        {
            var updateStockContractFunction = new UpdateStockContractFunction();
            updateStockContractFunction.NewContract = newContract;

            return ContractHandler.SendRequestAsync(updateStockContractFunction);
        }

        public Task<TransactionReceipt> UpdateStockContractRequestAndWaitForReceiptAsync(string newContract, CancellationTokenSource cancellationToken = null)
        {
            var updateStockContractFunction = new UpdateStockContractFunction();
            updateStockContractFunction.NewContract = newContract;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(updateStockContractFunction, cancellationToken);
        }

        public Task<byte> GetOrderStatusQueryAsync(GetOrderStatusFunction getOrderStatusFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOrderStatusFunction, byte>(getOrderStatusFunction, blockParameter);
        }


        public Task<byte> GetOrderStatusQueryAsync(uint orderId, BlockParameter blockParameter = null)
        {
            var getOrderStatusFunction = new GetOrderStatusFunction();
            getOrderStatusFunction.OrderId = orderId;

            return ContractHandler.QueryAsync<GetOrderStatusFunction, byte>(getOrderStatusFunction, blockParameter);
        }

        public Task<string> ConfirmDeliveryRequestAsync(ConfirmDeliveryFunction confirmDeliveryFunction)
        {
            return ContractHandler.SendRequestAsync(confirmDeliveryFunction);
        }

        public Task<TransactionReceipt> ConfirmDeliveryRequestAndWaitForReceiptAsync(ConfirmDeliveryFunction confirmDeliveryFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(confirmDeliveryFunction, cancellationToken);
        }

        public Task<string> ConfirmDeliveryRequestAsync(uint orderId)
        {
            var confirmDeliveryFunction = new ConfirmDeliveryFunction();
            confirmDeliveryFunction.OrderId = orderId;

            return ContractHandler.SendRequestAsync(confirmDeliveryFunction);
        }

        public Task<TransactionReceipt> ConfirmDeliveryRequestAndWaitForReceiptAsync(uint orderId, CancellationTokenSource cancellationToken = null)
        {
            var confirmDeliveryFunction = new ConfirmDeliveryFunction();
            confirmDeliveryFunction.OrderId = orderId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(confirmDeliveryFunction, cancellationToken);
        }

        public Task<string> AcceptOrderRequestAsync(AcceptOrderFunction acceptOrderFunction)
        {
            return ContractHandler.SendRequestAsync(acceptOrderFunction);
        }

        public Task<TransactionReceipt> AcceptOrderRequestAndWaitForReceiptAsync(AcceptOrderFunction acceptOrderFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(acceptOrderFunction, cancellationToken);
        }

        public Task<string> AcceptOrderRequestAsync(uint orderId)
        {
            var acceptOrderFunction = new AcceptOrderFunction();
            acceptOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAsync(acceptOrderFunction);
        }

        public Task<TransactionReceipt> AcceptOrderRequestAndWaitForReceiptAsync(uint orderId, CancellationTokenSource cancellationToken = null)
        {
            var acceptOrderFunction = new AcceptOrderFunction();
            acceptOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(acceptOrderFunction, cancellationToken);
        }

        public Task<List<uint>> GetNewOrdersQueryAsync(GetNewOrdersFunction getNewOrdersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetNewOrdersFunction, List<uint>>(getNewOrdersFunction, blockParameter);
        }


        public Task<List<uint>> GetNewOrdersQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetNewOrdersFunction, List<uint>>(null, blockParameter);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
            return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;

            return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
            transferOwnershipFunction.NewOwner = newOwner;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> DeliverOrderRequestAsync(DeliverOrderFunction deliverOrderFunction)
        {
            return ContractHandler.SendRequestAsync(deliverOrderFunction);
        }

        public Task<TransactionReceipt> DeliverOrderRequestAndWaitForReceiptAsync(DeliverOrderFunction deliverOrderFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(deliverOrderFunction, cancellationToken);
        }

        public Task<string> DeliverOrderRequestAsync(uint orderId)
        {
            var deliverOrderFunction = new DeliverOrderFunction();
            deliverOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAsync(deliverOrderFunction);
        }

        public Task<TransactionReceipt> DeliverOrderRequestAndWaitForReceiptAsync(uint orderId, CancellationTokenSource cancellationToken = null)
        {
            var deliverOrderFunction = new DeliverOrderFunction();
            deliverOrderFunction.OrderId = orderId;

            return ContractHandler.SendRequestAndWaitForReceiptAsync(deliverOrderFunction, cancellationToken);
        }
    }
}
