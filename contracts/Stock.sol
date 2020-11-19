pragma solidity >=0.5.0;

import "./openZeppelin/ownership/Ownable.sol";
import "./openZeppelin/access/roles/WhitelistedRole.sol";

contract Stock is WhitelistedRole {
    struct Item {
        string name;
        string description;
        string category;
        uint32 stock;
    }

    // Item creation event
    event ItemCreated(uint32 id, string message);

    // Item removal event
    event ItemRemoved(uint32 id, string message);

    // Item property update events
    event ItemStockUpdated(uint32 id, uint32 stock, string message);
    event ItemFieldUpdated(uint32 id, string field, string message);

    mapping(uint32 => Item) private items;
    uint32 private itemCount;

    function itemExists(uint32 id) private view returns (bool) {
        return bytes(items[id].name).length != 0;
    }

    function createItem(
        string memory name,
        string memory description,
        string memory category,
        uint32 stock
    ) public onlyWhitelisted returns (uint32) {
        uint32 itemId = itemCount;
        Item storage currentItem = items[itemId];

        require(!itemExists(itemId), "Item already exists.");

        currentItem.name = name;
        currentItem.description = description;
        currentItem.category = category;
        currentItem.stock = stock;

        itemCount = itemCount + 1;

        emit ItemCreated(itemId, "Item created.");

        return itemId;
    }

    function removeItem(uint32 id) public onlyWhitelisted {
        require(itemExists(id), "Item does not exist.");

        delete (items[id]);

        emit ItemRemoved(id, "Item removed.");
    }

    function getItem(uint32 id)
        public
        view
        returns (
            string memory name,
            string memory description,
            string memory category,
            uint32 stock
        )
    {
        require(itemExists(id), "Item does not exist.");
        Item memory item = items[id];

        return (item.name, item.description, item.category, item.stock);
    }

    function getItemStock(uint32 id) public view returns (uint32 stock) {
        require(itemExists(id), "Item does not exist.");

        return items[id].stock;
    }

    function increaseItemStock(uint32 id, uint32 amount)
        public
        onlyWhitelisted
    {
        require(itemExists(id), "Item does not exist.");

        uint32 newStock = items[id].stock + amount;

        require(
            newStock >= items[id].stock,
            "Failed to increase item stock because of overflow."
        );

        items[id].stock = newStock;

        emit ItemStockUpdated(id, items[id].stock, "Item stock increased.");
    }

    function decreaseItemStock(uint32 id, uint32 amount)
        public
        onlyWhitelisted
    {
        require(itemExists(id), "Item does not exist.");
        require(
            amount <= items[id].stock,
            "Failed to decrease item stock: stock must be equal to or greater than 0."
        );

        items[id].stock = items[id].stock - amount;

        emit ItemStockUpdated(id, items[id].stock, "Item stock decreased.");
    }

    function updateItemName(uint32 id, string memory name)
        public
        onlyWhitelisted
    {
        require(itemExists(id), "Item does not exist.");

        items[id].name = name;

        emit ItemFieldUpdated(id, name, "Item name updated.");
    }

    function updateItemDescription(uint32 id, string memory description)
        public
        onlyWhitelisted
    {
        require(itemExists(id), "Item does not exist.");

        items[id].description = description;

        emit ItemFieldUpdated(id, description, "Item description updated.");
    }

    function updateItemCategory(uint32 id, string memory category)
        public
        onlyWhitelisted
    {
        require(itemExists(id), "Item does not exist.");

        items[id].category = category;

        emit ItemFieldUpdated(id, category, "Item category updated.");
    }
}
