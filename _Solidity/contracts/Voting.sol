// SPDX-License-Identifier: SEE LICENSE IN LICENSE
pragma solidity ^0.8.0;

contract Voting {
    struct Candidate {
        uint256 id;
        string party;
        uint256 votes;
    }
    address internal owner;
    mapping(string => bool) internal votedCandidates; //VoterId==>isVoted?
    mapping(address => bool) internal votedCandidateAdresses;
    //mapping (address=>string) internal adddressToVoterId;
    //mapping(Candidate => uint256) participatingParties; //Candidate ==>voteCount
    Candidate[] internal participatingParties;
    uint256[] internal candidateIDs;

    uint256 internal electionStartTime = 0;
    uint256 internal electionEndTime = 0;
    uint256 internal electionResultTime = 0;

    constructor() {
        owner = msg.sender;
    }

    function startElection(
        uint256 _electionStartTime,
        uint256 _electionEndTime,
        uint256 _electionResultTime
    ) public {
        electionStartTime = _electionStartTime;
        electionEndTime = _electionEndTime;
        electionResultTime = _electionResultTime;
    }

    function getElectionTiming() public view returns (electionTiming memory) {
        return
            electionTiming(
                electionStartTime,
                electionEndTime,
                electionResultTime
            );
    }

    struct electionTiming {
        uint256 electionStartTime;
        uint256 electionEndTime;
        uint256 electionResultTime;
    }

    function addCandidate(
        uint256 id,
        string memory partySymbol
    ) public onlyOwner returns (bool) {
        require(isElectionYetToStart(), "Can't add! Election is started..");
        require(
            !arrayHasValue(candidateIDs, id),
            "Duplicate Candidate ID Not Allowed"
        );
        participatingParties.push(Candidate(id, partySymbol, 0));
        candidateIDs.push(id);
        return true;
    }

    function recordVote(
        string memory voterID,
        uint256 candidateID
    ) public onlyOneVoteAllowed(voterID) returns (bool) {
        require(!isElectionYetToStart(), "Election is not started yet");
        require(isElectionRunning(), "Election is ended");
        require(isValidCandidateId(candidateID), "Invalid Candidate ID");
        for (uint i = 0; i < participatingParties.length; i++) {
            if (participatingParties[i].id == candidateID) {
                participatingParties[i].votes++;
                votedCandidates[voterID] = true;
                votedCandidateAdresses[msg.sender] = true;
                return true;
            }
        }
        return false;
    }

    function getVoterStatus(string memory voterId) public view returns (bool) {
        return votedCandidates[voterId];
    }

    function getWinnerInElection() public view returns (Candidate memory) {
        require(isElectionEnded(), "Election is not completed yet!");
        Candidate memory winner = participatingParties[0];
        for (uint i = 1; i < participatingParties.length; i++) {
            if (
                keccak256(abi.encodePacked(participatingParties[i].party)) ==
                keccak256(abi.encodePacked("NOTA"))
            ) continue;
            if (participatingParties[i].votes > winner.votes) {
                winner = participatingParties[i];
            }
            //What if multiple parties got same number of votes???
            //what if no one voted to any Candidate?
        }
        return winner;
    }

    function getCandidates() public view returns (Candidate[] memory) {
        return participatingParties;
    }

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }

    modifier onlyOneVoteAllowed(string memory voterID) {
        require(
            !votedCandidates[voterID] && !votedCandidateAdresses[msg.sender],
            "Double Vote Not Allowed!"
        );
        _;
    }

    function arrayHasValue(
        uint256[] memory arr,
        uint256 value
    ) internal pure returns (bool) {
        for (uint8 i = 0; i < arr.length; i++) {
            if (arr[i] == value) return true;
        }
        return false;
    }

    function isElectionRunning() internal view returns (bool) {
        return (electionStartTime <= block.timestamp &&
            electionEndTime > block.timestamp);
    }

    function isElectionYetToStart() internal view returns (bool) {
        return electionStartTime > block.timestamp;
    }

    function isElectionEnded() internal view returns (bool) {
        return block.timestamp > electionEndTime;
    }

    function isValidCandidateId(
        uint256 candidateId
    ) internal view returns (bool) {
        for (uint256 i = 0; i < candidateIDs.length; i++) {
            if (candidateIDs[i] == candidateId) return true;
        }
        return false;
    }
}
