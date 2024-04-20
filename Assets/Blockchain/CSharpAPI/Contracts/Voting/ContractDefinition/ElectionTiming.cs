using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace _Solidity.Contracts.Voting.ContractDefinition
{
    public partial class ElectionTiming : ElectionTimingBase { }

    public class ElectionTimingBase 
    {
        [Parameter("uint256", "electionStartTime", 1)]
        public virtual BigInteger ElectionStartTime { get; set; }
        [Parameter("uint256", "electionEndTime", 2)]
        public virtual BigInteger ElectionEndTime { get; set; }
        [Parameter("uint256", "electionResultTime", 3)]
        public virtual BigInteger ElectionResultTime { get; set; }
    }
}
