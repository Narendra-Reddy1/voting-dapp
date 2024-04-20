const { hre, ethers } = require("hardhat");
require("dotenv").config();
async function main() {

    const provider = new ethers.JsonRpcProvider(process.env.LOCAL_HOST_RPC_URL);
    const wallet = new ethers.Wallet(process.env.PRIVATE_KEY, provider);

    const contract = await ethers.getContractFactory("Voting", wallet);
    const deployedContract = await contract.deploy();
    const response = await deployedContract.waitForDeployment();
    console.log(response);

}

main().catch((e) => { console.log(e) }).then(() => {
    process.exitCode = 1;
})