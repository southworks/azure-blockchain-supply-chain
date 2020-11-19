var Roles = artifacts.require("Roles");
var WhitelistedRole = artifacts.require("WhitelistedRole");

module.exports = deployer => {
    deployer.deploy(Roles);
    deployer.link(Roles, WhitelistedRole);
    deployer.link(Roles, WhitelistedRole);
    deployer.deploy(WhitelistedRole);
};