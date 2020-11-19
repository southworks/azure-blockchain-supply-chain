const HDWalletProvider = require("truffle-hdwallet-provider");
const fs = require("fs");
const path = require('path');
require('dotenv').config();

const { RPC_PATH } = process.env;

module.exports = {
  networks: {
    development: {
      host: "127.0.0.1",
      port: 9545,
      network_id: "*"
    },
    abs_consortium: {
      network_id: "*",
      gasPrice: 0,
      provider: new HDWalletProvider(fs.readFileSync(path.resolve(__dirname, './mnemonic.env'), 'utf-8'), RPC_PATH)
    }
  }
};
