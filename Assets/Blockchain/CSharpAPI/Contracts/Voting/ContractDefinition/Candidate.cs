using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace _Solidity.Contracts.Voting.ContractDefinition
{
    public partial class Candidate : CandidateBase { }

    public class CandidateBase 
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("string", "party", 2)]
        public virtual string Party { get; set; }
        [Parameter("uint256", "votes", 3)]
        public virtual BigInteger Votes { get; set; }
    }
}
