
const Stock = artifacts.require("Stock");

contract("Stock test", (accounts) => {
    let instance = null;
    let itemName = "Item";
    let itemDescription = "Item description";
    let itemCategory = "Test";
    let itemStock = 10;
    let testId = 0;
    let amount = 20;

    before(async function () {
        instance = await Stock.new();
        await instance.addWhitelisted(accounts[0]);
    });

    it('should allow to the owner to create an item', async () => {
        await instance.createItem(itemName, itemDescription, itemCategory, itemStock).then((res, err) => {
            if (!err) {
                let log = res.logs[0];
                let id = log.args.id.toNumber();
                assert.equal(id, testId, "Item created");
            } else {
                assert.isFalse(true, "Creation failed");
            }
        });
    });

    it('should allow to get an item', async () => {
        let primerItemName = await instance.getItem(testId).then(
            res => res.name,
            err => 'id undefined'
        );
        assert.isNotEmpty(primerItemName, "Item[" + testId + "] successfully getted");
    });

    it("should allow to get an item's stock", async () => {
        let primerItemStock = (await instance.getItemStock(testId)).toNumber();
        assert.equal(primerItemStock, itemStock, "Item[" + testId + "] stock getted");
    });

    it("should allow to the owner to update item's name", async function () {
        await instance.updateItemName(testId, 'New name');
        let primerItemName = await instance.getItem(testId).then(
            res => res.name,
            err => 'id undefined'
        );
        assert.equal(primerItemName, 'New name');
    });

    it("should allow to the owner to update item's description", async function () {
        await instance.updateItemDescription(testId, 'New Description');
        let primerItemDescription = await instance.getItem(testId).then(
            res => res.description,
            err => 'id undefined'
        );
        assert.equal(primerItemDescription, 'New Description');
    });

    it("should allow to the owner to update item's category", async function () {
        await instance.updateItemCategory(testId, 'New Category');
        let primerItemCategory = await instance.getItem(testId).then(
            res => res.category,
            err => 'id undefined'
        );
        assert.equal(primerItemCategory, 'New Category');
    });

    it("should allow to the owner to increase item's stock", async function () {
        await instance.increaseItemStock(testId, amount);
        let primerItemStock = await instance.getItem(testId).then(
            res => res.stock,
            err => 'id undefined'
        );
        assert.equal(primerItemStock, itemStock + amount);
    });

    it("should allow to the owner to decrease item's stock", async function () {
        await instance.decreaseItemStock(testId, amount);
        let primerItemStock = await instance.getItem(testId).then(
            res => res.stock,
            err => 'id undefined'
        );
        assert.equal(primerItemStock, itemStock);
    });

    it('should allow to the owner to remove an item', async () => {
        await instance.removeItem(testId);
        let primerItemName = await instance.getItem(testId).then(
            res => res.name,
            err => 'id undefined'
        );
        assert.equal(primerItemName, 'id undefined', "Item[" + testId + "] removed");
    });

});