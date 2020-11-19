const ethers = require('ethers');
const fs = require("fs");
const path = require('path');
let mnemonic = fs.readFileSync(path.resolve(__dirname, './mnemonic.env'), 'utf-8');
let mnemonicWallet = ethers.Wallet.fromMnemonic(mnemonic);
console.log(mnemonicWallet.privateKey);