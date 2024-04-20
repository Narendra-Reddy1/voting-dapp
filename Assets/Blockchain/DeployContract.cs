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
    public string start;
    public string end;
    public string result;
    public string abiPath;
    public string byteCodePath;
    public string gas = "30000000";
    public string gasPrice = "2000000";
    public string contractAddress = "0x71c95911e9a5d330f4d621842ec243ee1343292e";
    public List<CandidateData> candidateList = new List<CandidateData>();
    [System.Serializable]
    public struct CandidateData
    {
        public int id;
        public string symbol;
    }

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

    [ContextMenu("StartElection")]
    public async void StartElection()
    {
        string abi;
        using (System.IO.StreamReader reader = new System.IO.StreamReader(abiPath, System.Text.Encoding.UTF8))
        {
            abi = (await reader.ReadToEndAsync());
        }
        var web3 = new Web3(Konstants.RPC_URL);
        web3.TransactionManager.DefaultGas = BigInteger.Parse(gas);
        web3.TransactionManager.DefaultGasPrice = BigInteger.Parse(gasPrice);
        VotingContract votingContract = new VotingContract(contractAddress, abi, web3);
        votingContract.StartElectionAsync(BigInteger.Parse(start), BigInteger.Parse(end), BigInteger.Parse(result));
    }
    [ContextMenu("AddCandidate")]
    public async void AddCandidate()
    {
        string abi;
        using (System.IO.StreamReader reader = new System.IO.StreamReader(abiPath, System.Text.Encoding.UTF8))
        {
            abi = (await reader.ReadToEndAsync());
        }
        var web3 = new Web3(Konstants.RPC_URL);
        web3.TransactionManager.DefaultGas = BigInteger.Parse(gas);
        web3.TransactionManager.DefaultGasPrice = BigInteger.Parse(gasPrice);
        VotingContract votingContract = new(contractAddress, abi, web3);
        foreach (CandidateData candidate in candidateList)
        {
            Debug.Log("adding candidate....");
            await votingContract.AddCandidateAsync(BigInteger.Parse(candidate.id.ToString()), candidate.symbol);
            Debug.Log("add candidate successful....");
        }
    }

    [ContextMenu("electiontiming")]
    public async void ElectionTiming()
    {
        string abi;
        using (System.IO.StreamReader reader = new System.IO.StreamReader(abiPath, System.Text.Encoding.UTF8))
        {
            abi = (await reader.ReadToEndAsync());
        }
        VotingContract contract = new VotingContract(contractAddress, abi, new Web3(Konstants.RPC_URL));
        contract.GetElectionTimingAsync();
    }


}


public class VotingContract
{
    // Define the function to get the winner
    [Function("getWinnerInElection", "tuple(uint256,string,uint256)")]
    public class GetWinnerFunction : FunctionMessage
    {
    }

    //[Function()]
    //public class StartElectionFunction : FunctionMessage { }
    // Define the contract object
    private Contract contract;
    private Web3 web3;
    private string contractaddress;
    // Constructor to initialize the contract object
    public VotingContract(string contractAddress, string abi, Web3 web3)
    {
        this.web3 = web3;
        this.contractaddress = contractAddress;
        contract = web3.Eth.GetContract(abi, contractAddress);
    }
    public async void StartElectionAsync(BigInteger start, BigInteger end, BigInteger result)
    {
        var function = contract.GetFunction("startElection");

        // Prepare the arguments
        var arg1 = new HexBigInteger(start); // Example uint256 argument 1
        var arg2 = new HexBigInteger(end); // Example uint256 argument 2
        var arg3 = new HexBigInteger(result); // Example uint256 argument 3

        // Send the transaction to the network
        var txHash = await function.SendTransactionAsync(Konstants.PUBLIC_KEY, start, end, result);

        // Wait for the transaction receipt
        var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);

        // Transaction receipt will contain the status of the transaction
        if (receipt.Status == new HexBigInteger(1))
        {
            Debug.Log("Transaction succeeded!");
        }
        else
        {
            Debug.LogError("Transaction failed!");
        }
    }

    public async void GetElectionTimingAsync()
    {
        var data = await contract.GetFunction("getElectionTiming").CallAsync<(BigInteger, BigInteger, BigInteger)>();
        Debug.Log(data.Item1);
    }


    public async Task<bool> AddCandidateAsync(BigInteger id, string partySymbol)
    {
        var result = await contract.GetFunction("addCandidate").CallAsync<bool>(id, partySymbol);
        Debug.Log(result);
        return result;
    }
    // Method to call the getWinnerInElection function
    public async Task<(BigInteger, string, BigInteger)> GetWinnerAsync()
    {
        (BigInteger, string, BigInteger) s = default;
        try
        {
            var getWinnerFunction = new GetWinnerFunction();
            s = await contract.GetFunction<GetWinnerFunction>().CallAsync<(BigInteger, string, BigInteger)>();
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
