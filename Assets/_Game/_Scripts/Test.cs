using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.HdWallet;
using com.cyborgAssets.inspectorButtonPro;
using _Solidity.Contracts.Voting;
using _Solidity.Contracts.Voting.ContractDefinition;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding;

namespace TestScript
{
    public class Test : MonoBehaviour
    {
        //public string myPublicKey;
        //public string myPrivateKey;
        //public string toAddress;

        public string startTime;
        public string endTime;
        public string resultTime;

        public List<CandidateData> candidateList;
        //wallet
        //0x5B38Da6a701c568545dCfcB03FcB875f56beddC4
        //contract address.
        //0x047b37Ef4d76C2366F795Fb557e3c15E0607b7d8
        private VotingService votingService;
        [ProButton]
        private async void _InitializeVotingService()
        {
            Account account = new Account("0x59c6995e998f97a5a0044966f0945389dc9e86dae88c7a8412f4603b6b78690d");
            Web3 web3 = new Web3(account);
            votingService = new VotingService(web3, "0x8464135c8f25da09e49bc8782676a84730c318bc");
            //StartElectionFunction startElection = new StartElectionFunction();
            //startElection.ElectionStartTime = BigInteger.Parse(startTime);
            //startElection.ElectionEndTime = BigInteger.Parse(endTime);
            //startElection.ElectionResultTime = BigInteger.Parse(resultTime);
            //var transaction = await votingService.StartElectionRequestAndWaitForReceiptAsync(startElection);
            //Debug.Log(transaction.Status);
            try
            {

                Debug.Log("Adding Candidates....");
                foreach (CandidateData candidate in candidateList)
                {
                    AddCandidateFunction candidate1 = new AddCandidateFunction();
                    candidate1.Id = BigInteger.Parse(candidate.id);
                    candidate1.PartySymbol = candidate.partySymbol;
                    var receipt = await votingService.AddCandidateRequestAndWaitForReceiptAsync(candidate1);
                    Debug.Log(receipt.Status);
                }
            }
            catch (SmartContractRevertException e)
            {
                Debug.Log(e);
                Debug.Log(e.RevertMessage);
            }
            //Debug.Log("Retrieving candidates");
            //GetCandidatesFunction getCandidates = new GetCandidatesFunction();
            //var result=await votingService.GetCandidatesQueryAsync(getCandidates);
            //foreach(Candidate c in result.ReturnValue1)
            //{
            //    Debug.Log(" ");
            //    Debug.Log(c.Id);
            //    Debug.Log(c.Party);
            //    Debug.Log("_________________________________________________________________________");
            //}
        }
    }

    [System.Serializable]
    public struct CandidateData
    {
        public string id;
        public string partySymbol;
    }
}