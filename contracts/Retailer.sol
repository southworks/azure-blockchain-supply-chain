pragma solidity >=0.5.0;

import "./Stock.sol";
import "./openZeppelin/ownership/Ownable.sol";

contract Retailer is Ownable {
    Stock private stock;
    uint32 private minItemsPerOrder;

    constructor(address stockContractAddress) public {
        stock = Stock(stockContractAddress);
    }

    enum OrderStatus {
        None,
        New,
        Cancelled,
        Accepted,
        Rejected,
        InTransit,
        Delivered
    }

    struct OrderItem {
        uint32 itemId;
        uint32 quantity;
    }

    struct Order {
        address clientAddress;
        string clientEmail;
        OrderStatus status;
        mapping(uint32 => OrderItem) items;
        uint256 itemCount;
    }

    mapping(uint32 => Order) public orders;
    uint32 orderIdCount;

     event OrderCreated(
        uint32 orderId,
        OrderStatus status,
        string clientEmail,
        string message
    );
     event OrderAccepted(
        uint32 orderId,
        OrderStatus status,
        string clientEmail,
        string message
    );
     event OrderCancelled(
        uint32 orderId,
        OrderStatus status,
        string clientEmail,
        string message
    );
    event OrderRejected(
        uint32 orderId,
        OrderStatus status,
        string clientEmail,
        string message
    );
     event OrderDelivered(
        uint32 orderId,
        OrderStatus status,
        string clientEmail,
        string message
    );
     event DeliveryConfirmed(
        uint32 orderId,
        OrderStatus status,
        string clientEmail,
        string message
    );
    
    // for testing
    function getOrderItem(uint32 orderId, uint32 itemId)
        public
        view
        returns (uint32 id, uint32 itemQty)
    {
        return (
            orders[orderId].items[itemId].itemId,
            orders[orderId].items[itemId].quantity
        );
    }

    function orderExists(uint32 orderId) private view returns (bool) {
        return (orders[orderId].status != OrderStatus.None);
    }

    function createOrder(
        uint32[] memory itemIds,
        uint32[] memory itemQuantities,
        string memory clientEmail
    ) public returns (uint32) {
        require(
            itemIds.length == itemQuantities.length,
            "Arrays for item IDs and quantities must be the same length."
        );

        uint256 itemCount = itemIds.length;

        for (uint32 i = 0; i < itemCount; i++) {
            require(
                itemQuantities[i] >= minItemsPerOrder,
                "Item quantity must be greater or equal to minItemsPerOrder."
            );
            require(
                stock.getItemStock(itemIds[i]) >= itemQuantities[i],
                "Item does not have enough stock."
            );
        }

        // Check if there are any duplicated item ids?

        uint32 orderId = orderIdCount;
        Order storage currentOrder = orders[orderId];

        currentOrder.clientAddress = msg.sender;
        currentOrder.clientEmail = clientEmail;
        currentOrder.status = OrderStatus.New;

        for (uint32 i = 0; i < itemCount; i++) {
            currentOrder.items[i].itemId = itemIds[i];
            currentOrder.items[i].quantity = itemQuantities[i];
        }

        currentOrder.itemCount = itemCount;

        orderIdCount++;

         emit OrderCreated(
            orderId,
            orders[orderId].status,
            orders[orderId].clientEmail,
            "Order Created"
        );

        return orderId;
    }

    function getOrderStatus(uint32 orderId) public view returns (OrderStatus) {
        require(orderExists(orderId), "Order does not exist.");
        require(
            msg.sender == owner() ||
                msg.sender == orders[orderId].clientAddress,
            "Only the contract owner or the client who placed the order can perform this operation."
        );

        return orders[orderId].status;
    }

    function checkOrderItemsAvailability(uint32 orderId)
        public
        view
        onlyOwner
        returns (bool)
    {
        require(orderExists(orderId), "Order does not exist.");
        bool itemsAvailable = true;

        for (uint32 i = 0; i < orders[orderId].itemCount; i++) {
            if (
                stock.getItemStock(orders[orderId].items[i].itemId) <
                orders[orderId].items[i].quantity
            ) {
                itemsAvailable = false;
                break;
            }
        }

        return itemsAvailable;
    }

    function cancelOrder(uint32 orderId) public {
        require(orderExists(orderId), "Order does not exist.");
        require(
            msg.sender == orders[orderId].clientAddress,
            "Only the client who placed the order can perform this operation."
        );
        require(
            orders[orderId].status == OrderStatus.New,
            "Order status must be New in order to get cancelled."
        );

        orders[orderId].status = OrderStatus.Cancelled;

        emit OrderCancelled
        (orderId, 
        orders[orderId].status, 
        orders[orderId].clientEmail,
        "Order cancelled");
    }

    function getNewOrders() public view onlyOwner returns (uint32[] memory) {
        uint32 newOrderCount;

        for (uint32 i = 0; i < orderIdCount; i++) {
            if (orders[i].status == OrderStatus.New) {
                newOrderCount++;
            }
        }

        uint32[] memory newOrders = new uint32[](newOrderCount);
        uint32 orderCount;

        for (uint32 i = 0; i < orderIdCount; i++) {
            if (orders[i].status == OrderStatus.New) {
                newOrders[orderCount++] = i;
            }
        }

        return newOrders;
    }

    function acceptOrder(uint32 orderId) public onlyOwner {
        require(orderExists(orderId), "Order does not exist.");
        require(
            orders[orderId].status == OrderStatus.New,
            "Order status must be New in order to be accepted."
        );
        require(
            checkOrderItemsAvailability(orderId),
            "At least one item in the current order is not available."
        );

        for (uint32 i = 0; i < orders[orderId].itemCount; i++) {
            stock.decreaseItemStock(
                orders[orderId].items[i].itemId,
                orders[orderId].items[i].quantity
            );
        }

        orders[orderId].status = OrderStatus.Accepted;

         emit OrderAccepted(
            orderId,
            orders[orderId].status,
            orders[orderId].clientEmail,
            "Order Accepted"
        );
    }

    function deliverOrder(uint32 orderId) public onlyOwner {
        require(orderExists(orderId), "Order does not exist.");
        require(
            orders[orderId].status == OrderStatus.Accepted,
            "Order status must be Accepted in order to be delivered."
        );

        orders[orderId].status = OrderStatus.InTransit;

        emit OrderDelivered(
            orderId,
            orders[orderId].status,
            orders[orderId].clientEmail,
            "Order delivered"
        );
    }

    function rejectOrder(uint32 orderId) public onlyOwner {
        require(orderExists(orderId), "Order does not exist.");
        require(
            orders[orderId].status == OrderStatus.New,
            "Order status must be New in order to get rejected."
        );

        orders[orderId].status = OrderStatus.Rejected;

         emit OrderRejected(
            orderId,
            orders[orderId].status,
            orders[orderId].clientEmail,
            "Order rejected"
        );
    }

    function confirmDelivery(uint32 orderId) public {
        require(orderExists(orderId), "Order does not exist.");
        require(
            msg.sender == orders[orderId].clientAddress,
            "Only the client who placed the order can perform this operation."
        );
        require(
            orders[orderId].status == OrderStatus.InTransit,
            "Order status must be InTransit in order to be confirmed."
        );

        orders[orderId].status = OrderStatus.Delivered;

         emit DeliveryConfirmed(
            orderId,
            orders[orderId].status,
            orders[orderId].clientEmail,
            "Order Delivery confirmed"
        );
    }

    function updateMinItemsPerOrder(uint32 minItems) public onlyOwner {
        minItemsPerOrder = minItems;
    }

    function updateStockContract(address newContract) public onlyOwner {
        stock = Stock(newContract);
    }
}
