using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.HdWallet;

public class Test : MonoBehaviour
{
    public string myPublicKey;
    public string myPrivateKey;
    public string toAddress;

    
    public async void TestMethod()
    {
        var account = new Account(myPrivateKey);
        var web3 = new Web3(account);
        var bal = await web3.Eth.GetBalance.SendRequestAsync(myPublicKey);
        Debug.Log(bal);

    }
}