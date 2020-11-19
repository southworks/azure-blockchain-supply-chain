let Stock = artifacts.require("Stock");
let Retailer = artifacts.require("Retailer");

module.exports = (deployer, networks, accounts) => {
    deployer.deploy(Stock).then((stockInstance) => {
        return deployer.deploy(Retailer, Stock.address).then(() => {
            stockInstance.addWhitelisted(accounts[0]);
            stockInstance.addWhitelisted(Retailer.address);
        });
    });
};