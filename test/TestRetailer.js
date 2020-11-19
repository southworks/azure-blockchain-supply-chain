const Stock = artifacts.require("Stock");
const Retailer = artifacts.require("Retailer");

contract("Retailer test", (accounts) => {
	const items = [];
	const MAX_ITEMS = 3;
	const OrderStatus = { None: 0, New: 1, Cancelled: 2, Accepted: 3, Rejected: 4, InTransit: 5, Delivered: 6 };

	let stockInstance;
	let retailerInstance;
	let testEmail = "test@test.com";

	for (let i = 0; i < MAX_ITEMS; i++) {
		items.push({
			name: `Item ${i}`,
			description: `Item ${i} description`,
			category: `Category ${i}`,
			stock: (i + 1) * 1000
		});
	}

	beforeEach(async function () {
		stockInstance = await Stock.new();
		await stockInstance.addWhitelisted(accounts[0]);

		for (let i = 0; i < MAX_ITEMS; i++) {
			await stockInstance.createItem(items[i].name, items[i].description, items[i].category, items[i].stock);
		}

		retailerInstance = await Retailer.new(stockInstance.address);
		await stockInstance.addWhitelisted(retailerInstance.address);
	});

	it('should create an order with received item IDs and quantities', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		const results = (await retailerInstance.createOrder(itemIds, itemQuantities, testEmail)).logs[0].args;

		assert.equal(results.orderId, 0);
		assert.equal(results.status, OrderStatus.New);
		assert.equal(results.message, "Order created");
	});

	it('should not create order because item does not have enough stock', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 13300];

		try {
			await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Item does not have enough stock. -- Reason given: Item does not have enough stock..");
		}
	});

	it('should get the order\'s status', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		const orderStatus = (await retailerInstance.getOrderStatus(0)).toNumber();

		assert.equal(orderStatus, OrderStatus.New);
	});

	it('should return true because all items in the order are available', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		assert.equal(await retailerInstance.checkOrderItemsAvailability(0), true);
	});

	it('should return false because not all items in the order are available', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [1000, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		await stockInstance.decreaseItemStock(0, 1);

		assert.equal(await retailerInstance.checkOrderItemsAvailability(0), false);
	});

	it('should change the order\'s status to Cancelled', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		const results = (await retailerInstance.cancelOrder(0)).logs[0].args;

		assert.equal(results.orderId, 0);
		assert.equal(results.status, OrderStatus.Cancelled);
		assert.equal(results.message, "Order cancelled");
	});

	it('should not cancel order because status is already Cancelled', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		await retailerInstance.cancelOrder(0);

		try {
			await retailerInstance.cancelOrder(0);
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Order status must be New in order to get cancelled. -- Reason given: Order status must be New in order to get cancelled..");
		}
	});

	it('should get one new order ID', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		const newOrders = await retailerInstance.getNewOrders();

		assert.equal(newOrders.length, 1);
		assert.equal(newOrders[0], 0);
	});

	it('should not get new orders because caller is not the owner', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		try {
			await retailerInstance.getNewOrders({ from: accounts[1] });
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Ownable: caller is not the owner");
		}
	});

	it('should accept order and decrease items\'s stocks', async () => {
		const itemIds = [0, 1];
		const itemQuantities = [100, 200];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		const results = (await retailerInstance.acceptOrder(0)).logs[0].args;

		assert.equal(results.orderId, 0);
		assert.equal(results.status, OrderStatus.Accepted);
		assert.equal(results.message, "Order accepted");

		assert.equal(await stockInstance.getItemStock(0), 900);
		assert.equal(await stockInstance.getItemStock(1), 1800);
	});

	it('should not accept order because status is not New', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		await retailerInstance.cancelOrder(0);

		try {
			await retailerInstance.acceptOrder(0);
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Order status must be New in order to be accepted. -- Reason given: Order status must be New in order to be accepted..");
		}
	});

	it('should change order status to InTransit', async () => {
		const itemIds = [0, 1];
		const itemQuantities = [100, 200];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		await retailerInstance.acceptOrder(0);

		const results = (await retailerInstance.deliverOrder(0)).logs[0].args;

		assert.equal(results.orderId, 0);
		assert.equal(results.status, OrderStatus.InTransit);
		assert.equal(results.clientEmail, testEmail);
		assert.equal(results.message, "Order delivered");
	});

	it('should not deliver order because status is not Accepted', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		try {
			await retailerInstance.deliverOrder(0);
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Order status must be Accepted in order to be delivered. -- Reason given: Order status must be Accepted in order to be delivered..");
		}
	});

	it('should change the order\'s status to Rejected', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		const results = (await retailerInstance.rejectOrder(0)).logs[0].args;

		assert.equal(results.orderId, 0);
		assert.equal(results.status, OrderStatus.Rejected);
		assert.equal(results.message, "Order rejected");
	});

	it('should not reject order because status is not New', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		await retailerInstance.cancelOrder(0);

		try {
			await retailerInstance.rejectOrder(0);
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Order status must be New in order to get rejected. -- Reason given: Order status must be New in order to get rejected..");
		}
	});

	it('should change the order\'s status to Delivered', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		await retailerInstance.acceptOrder(0);

		await retailerInstance.deliverOrder(0);

		const results = (await retailerInstance.confirmDelivery(0)).logs[0].args;

		assert.equal(results.orderId, 0);
		assert.equal(results.status, OrderStatus.Delivered);
		assert.equal(results.message, "Delivery confirmed");
	});

	it('should not confirm delivery because status is not InTransit', async () => {
		const itemIds = [0, 1, 2];
		const itemQuantities = [100, 200, 133];

		await retailerInstance.createOrder(itemIds, itemQuantities, testEmail);

		try {
			await retailerInstance.confirmDelivery(0);
			assert.isFalse(true, "Expected to fail but did not throw an error.");
		} catch (error) {
			assert.equal(error.message, "Returned error: VM Exception while processing transaction: revert Order status must be InTransit in order to be confirmed. -- Reason given: Order status must be InTransit in order to be confirmed..");
		}
	});
});
