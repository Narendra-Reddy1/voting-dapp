require("@nomicfoundation/hardhat-toolbox");
require("dotenv").config();




/** @type import('hardhat/config').HardhatUserConfig */
module.exports = {
  solidity: "0.8.0",
  defaultNetwork: "localhost",
  networks: {
    "sepolia": {
      chainId: 11155111,
      accounts: [process.env.PRIVATE_KEY],
      url: process.env.SEPOLIA_RPC_URL,
    },
    "localhost": {
      chainId: 13373,
      accounts: ["0x59c6995e998f97a5a0044966f0945389dc9e86dae88c7a8412f4603b6b78690d"],//hardhat private key
      url: process.env.LOCAL_HOST_RPC_URL,
    }

  }
};
