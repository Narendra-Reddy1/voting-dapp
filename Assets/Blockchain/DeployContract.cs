using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Unity.Rpc;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class DeployContract : MonoBehaviour
{
    public string abiPath;
    public string byteCodePath;
    public string gas = "30000000";
    public string gasPrice = "2000000";
    public string contractAddress = "0x959922be3caee4b8cd9a407cc3ac1c251c2007b1";

    public async Task<string> Deploy()
    {

        try
        {
            string abi;
            string bytecode;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(abiPath, System.Text.Encoding.UTF8))
            {
                abi = (await reader.ReadToEndAsync());
            }
            using (System.IO.StreamReader reader = new System.IO.StreamReader(byteCodePath, System.Text.Encoding.UTF8))
            {

                bytecode = (await reader.ReadToEndAsync());
            }

            var web3 = new Web3(Konstants.RPC_URL);
            var account = new Account(Konstants.PRIVATE_KEY, 13373);
            web3.TransactionManager.DefaultGas = BigInteger.Parse(gas);
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse(gasPrice);

            //var deploymentMessage = new ContractDeploymentMessage(bytecode);
            //deploymentMessage.FromAddress = Konstants.PUBLIC_KEY;
            //deploymentMessage.Gas = new HexBigInteger(BigInteger.Parse(gas));
            //deploymentMessage.GasPrice = new HexBigInteger(BigInteger.Parse(gasPrice));


            //BigInteger startTime = 1713202080; // your epoch time value
            //BigInteger endTime = 1713203700; // your epoch time value
            //BigInteger resultTime = 1713203760; // your epoch time value
            //BigInteger[] param = new[] { startTime, endTime, resultTime };

            //var req = new TransactionSignedUnityRequest(Konstants.RPC_URL, Konstants.PRIVATE_KEY, BigInteger.Parse("13373"));
            //StartCoroutine(req.SignAndSendDeploymentContractTransaction(deploymentMessage));
            //var transactionReceipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(bytecode, Konstants.PUBLIC_KEY, param );
            var transactionReceipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(bytecode, Konstants.PUBLIC_KEY);
            Debug.Log($"Contract Deployed @::: {transactionReceipt.ContractAddress}");
            Debug.Log("_________________________________________________________");
            Debug.Log(JsonUtility.ToJson(transactionReceipt));
            return transactionReceipt.ContractAddress;

            return null;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    [ContextMenu("INteract")]
    public async void Interact()
    {
        string abi;
        using (System.IO.StreamReader reader = new System.IO.StreamReader(abiPath, System.Text.Encoding.UTF8))
        {
            abi = (await reader.ReadToEndAsync());
        }
        var web3 = new Web3(Konstants.RPC_URL);
        VotingContract votingContract = new VotingContract(contractAddress, abi, web3);
        var winner = await votingContract.GetWinnerAsync();
        Debug.Log(winner);
    }
}

public class VotingContract
{
    // Define the function to get the winner
    [Function("getWinnerInElection", "tuple(uint256,string,uint256)")]
    public class GetWinnerFunction : FunctionMessage
    {
    }

    // Define the contract object
    private Contract contract;

    // Constructor to initialize the contract object
    public VotingContract(string contractAddress, string abi, Web3 web3)
    {
        contract = web3.Eth.GetContract(abi, contractAddress);
    }

    // Method to call the getWinnerInElection function
    public async Task<(BigInteger, string, BigInteger)> GetWinnerAsync()
    {
        (BigInteger, string, BigInteger) s=default;
        try
        {
            var getWinnerFunction = new GetWinnerFunction();
             s =await contract.GetFunction<GetWinnerFunction>().CallAsync<(BigInteger, string, BigInteger)>();
            return s;
        }
        catch (SmartContractCustomErrorRevertException ex)
        {
            // Handle custom error message
            Debug.Log(ex);

        }
        catch (Exception ex)
        {
            // Handle other exceptions
            Debug.LogError($"An error occurred: {ex.Message}");

        }
            return s;
    }
}
