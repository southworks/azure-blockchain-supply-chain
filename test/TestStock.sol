pragma solidity >=0.5.0;

import "truffle/Assert.sol";
import "truffle/DeployedAddresses.sol";
import "../contracts/Stock.sol";

contract TestStock {
    function testCreateItem() public {
        Stock instance = new Stock();
        instance.addWhitelisted(address(this));

        uint32 expected = instance.createItem(
            "Item",
            "Item description",
            "Test",
            uint32(10)
        );

        string memory name;
        string memory description;
        string memory category;
        uint32 stock;
        (name, description, category, stock) = instance.getItem(expected);

        Assert.equal(name, "Item", "Item should have the correct name");
        Assert.equal(
            description,
            "Item description",
            "Item should have the correct description"
        );
        Assert.equal(category, "Test", "Item should have the correct category");
        Assert.equal(
            uint256(stock),
            uint256(10),
            "Item should have the correct stock"
        );
    }

    function testGetItem() public {
        Stock instance = new Stock();
        instance.addWhitelisted(address(this));

        uint32 id1 = instance.createItem(
            "Item",
            "Item description",
            "Test",
            uint32(10)
        );
        uint32 id2 = instance.createItem(
            "Item2",
            "Item description2",
            "Test2",
            uint32(20)
        );
        uint32 id3 = instance.createItem(
            "Item3",
            "Item description3",
            "Test3",
            uint32(15)
        );

        string memory name;
        string memory description;
        string memory category;
        uint32 stock;
        (name, description, category, stock) = instance.getItem(id1);

        Assert.equal(name, "Item", "Item should have the correct name");
        Assert.equal(
            description,
            "Item description",
            "Item should have the correct description"
        );
        Assert.equal(category, "Test", "Item should have the correct category");
        Assert.equal(
            uint256(stock),
            uint256(10),
            "Item should have the correct stock"
        );

        (name, description, category, stock) = instance.getItem(id2);

        Assert.equal(name, "Item2", "Item should have the correct name");
        Assert.equal(
            description,
            "Item description2",
            "Item should have the correct description"
        );
        Assert.equal(
            category,
            "Test2",
            "Item should have the correct category"
        );
        Assert.equal(
            uint256(stock),
            uint256(20),
            "Item should have the correct stock"
        );

        (name, description, category, stock) = instance.getItem(id3);

        Assert.equal(name, "Item3", "Item should have the correct name");
        Assert.equal(
            description,
            "Item description3",
            "Item should have the correct description"
        );
        Assert.equal(
            category,
            "Test3",
            "Item should have the correct category"
        );
        Assert.equal(
            uint256(stock),
            uint256(15),
            "Item should have the correct stock"
        );
    }

    function testGetItemStock() public {
        Stock instance = new Stock();
        instance.addWhitelisted(address(this));

        uint32 expected = instance.createItem(
            "Item",
            "Item description",
            "Test",
            uint32(10)
        );
        uint32 expectedTest2 = instance.createItem(
            "Item",
            "Item description",
            "Test",
            uint32(40)
        );

        Assert.equal(
            uint256(instance.getItemStock(expected)),
            uint256(10),
            "Item Stock should return 10"
        );
        Assert.equal(
            uint256(instance.getItemStock(expectedTest2)),
            uint256(40),
            "Item Stock should return 40"
        );
    }

    function testDecreaseItemStock() public {
        Stock instance = new Stock();
        instance.addWhitelisted(address(this));

        uint32 expected = instance.createItem(
            "Item",
            "Item description",
            "Test",
            uint32(10)
        );
        instance.decreaseItemStock(expected, 4);

        Assert.equal(
            instance.getItemStock(expected),
            uint256(6),
            "Stock should of decreased by 4"
        );
    }
}
