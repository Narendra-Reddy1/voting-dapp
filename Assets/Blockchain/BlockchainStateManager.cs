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
using _Solidity.Contracts.Voting;
using _Solidity.Contracts.Voting.ContractDefinition;
using Nethereum.ABI.FunctionEncoding;

public class BlockchainStateManager : MonoBehaviour
{
    public static BlockchainStateManager instance { get; private set; }
    private VotingService voting;
    public VotingService VotingService => voting;

    private void Awake()
    {
        instance = this;
    }

    public async Task<bool> IsCandidatedVotedAlready(string voterId)
    {
        GetVoterStatusFunction getVoterStatus = new GetVoterStatusFunction();
        return await voting.GetVoterStatusQueryAsync(getVoterStatus);
    }
    public async void StartElection(BigInteger startTime, BigInteger endTime, BigInteger resultTime,Action onSuccess=null, Action<string> onTransactionReverted = null)
    {
        try
        {
            StartElectionFunction startElection = new StartElectionFunction();
            startElection.ElectionStartTime = startTime;
            startElection.ElectionEndTime = endTime;
            startElection.ElectionResultTime = resultTime;
            await voting.StartElectionRequestAndWaitForReceiptAsync(startElection);
            onSuccess?.Invoke();
        }
        catch(SmartContractRevertException e)
        {
            onTransactionReverted?.Invoke(e.RevertMessage);
        }
    }
    public async void RecordVote()
    {

    }
    public void GetWinnerInElections()
    {

    }
    public void GetCandidatesList()
    {

    }

}
