using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using _Solidity.Contracts.Voting.ContractDefinition;

namespace _Solidity.Contracts.Voting
{
    public partial class VotingService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, VotingDeployment votingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<VotingDeployment>().SendRequestAndWaitForReceiptAsync(votingDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, VotingDeployment votingDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<VotingDeployment>().SendRequestAsync(votingDeployment);
        }

        public static async Task<VotingService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, VotingDeployment votingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, votingDeployment, cancellationTokenSource);
            return new VotingService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public VotingService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public VotingService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddCandidateRequestAsync(AddCandidateFunction addCandidateFunction)
        {
             return ContractHandler.SendRequestAsync(addCandidateFunction);
        }

        public Task<TransactionReceipt> AddCandidateRequestAndWaitForReceiptAsync(AddCandidateFunction addCandidateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addCandidateFunction, cancellationToken);
        }

        public Task<string> AddCandidateRequestAsync(BigInteger id, string partySymbol)
        {
            var addCandidateFunction = new AddCandidateFunction();
                addCandidateFunction.Id = id;
                addCandidateFunction.PartySymbol = partySymbol;
            
             return ContractHandler.SendRequestAsync(addCandidateFunction);
        }

        public Task<TransactionReceipt> AddCandidateRequestAndWaitForReceiptAsync(BigInteger id, string partySymbol, CancellationTokenSource cancellationToken = null)
        {
            var addCandidateFunction = new AddCandidateFunction();
                addCandidateFunction.Id = id;
                addCandidateFunction.PartySymbol = partySymbol;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addCandidateFunction, cancellationToken);
        }

        public Task<GetCandidatesOutputDTO> GetCandidatesQueryAsync(GetCandidatesFunction getCandidatesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetCandidatesFunction, GetCandidatesOutputDTO>(getCandidatesFunction, blockParameter);
        }

        public Task<GetCandidatesOutputDTO> GetCandidatesQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetCandidatesFunction, GetCandidatesOutputDTO>(null, blockParameter);
        }

        public Task<GetElectionTimingOutputDTO> GetElectionTimingQueryAsync(GetElectionTimingFunction getElectionTimingFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetElectionTimingFunction, GetElectionTimingOutputDTO>(getElectionTimingFunction, blockParameter);
        }

        public Task<GetElectionTimingOutputDTO> GetElectionTimingQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetElectionTimingFunction, GetElectionTimingOutputDTO>(null, blockParameter);
        }

        public Task<bool> GetVoterStatusQueryAsync(GetVoterStatusFunction getVoterStatusFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetVoterStatusFunction, bool>(getVoterStatusFunction, blockParameter);
        }

        
        public Task<bool> GetVoterStatusQueryAsync(string voterId, BlockParameter blockParameter = null)
        {
            var getVoterStatusFunction = new GetVoterStatusFunction();
                getVoterStatusFunction.VoterId = voterId;
            
            return ContractHandler.QueryAsync<GetVoterStatusFunction, bool>(getVoterStatusFunction, blockParameter);
        }

        public Task<GetWinnerInElectionOutputDTO> GetWinnerInElectionQueryAsync(GetWinnerInElectionFunction getWinnerInElectionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetWinnerInElectionFunction, GetWinnerInElectionOutputDTO>(getWinnerInElectionFunction, blockParameter);
        }

        public Task<GetWinnerInElectionOutputDTO> GetWinnerInElectionQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetWinnerInElectionFunction, GetWinnerInElectionOutputDTO>(null, blockParameter);
        }

        public Task<string> RecordVoteRequestAsync(RecordVoteFunction recordVoteFunction)
        {
             return ContractHandler.SendRequestAsync(recordVoteFunction);
        }

        public Task<TransactionReceipt> RecordVoteRequestAndWaitForReceiptAsync(RecordVoteFunction recordVoteFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(recordVoteFunction, cancellationToken);
        }

        public Task<string> RecordVoteRequestAsync(string voterID, BigInteger candidateID)
        {
            var recordVoteFunction = new RecordVoteFunction();
                recordVoteFunction.VoterID = voterID;
                recordVoteFunction.CandidateID = candidateID;
            
             return ContractHandler.SendRequestAsync(recordVoteFunction);
        }

        public Task<TransactionReceipt> RecordVoteRequestAndWaitForReceiptAsync(string voterID, BigInteger candidateID, CancellationTokenSource cancellationToken = null)
        {
            var recordVoteFunction = new RecordVoteFunction();
                recordVoteFunction.VoterID = voterID;
                recordVoteFunction.CandidateID = candidateID;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(recordVoteFunction, cancellationToken);
        }

        public Task<string> StartElectionRequestAsync(StartElectionFunction startElectionFunction)
        {
             return ContractHandler.SendRequestAsync(startElectionFunction);
        }

        public Task<TransactionReceipt> StartElectionRequestAndWaitForReceiptAsync(StartElectionFunction startElectionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startElectionFunction, cancellationToken);
        }

        public Task<string> StartElectionRequestAsync(BigInteger electionStartTime, BigInteger electionEndTime, BigInteger electionResultTime)
        {
            var startElectionFunction = new StartElectionFunction();
                startElectionFunction.ElectionStartTime = electionStartTime;
                startElectionFunction.ElectionEndTime = electionEndTime;
                startElectionFunction.ElectionResultTime = electionResultTime;
            
             return ContractHandler.SendRequestAsync(startElectionFunction);
        }

        public Task<TransactionReceipt> StartElectionRequestAndWaitForReceiptAsync(BigInteger electionStartTime, BigInteger electionEndTime, BigInteger electionResultTime, CancellationTokenSource cancellationToken = null)
        {
            var startElectionFunction = new StartElectionFunction();
                startElectionFunction.ElectionStartTime = electionStartTime;
                startElectionFunction.ElectionEndTime = electionEndTime;
                startElectionFunction.ElectionResultTime = electionResultTime;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startElectionFunction, cancellationToken);
        }
    }
}
