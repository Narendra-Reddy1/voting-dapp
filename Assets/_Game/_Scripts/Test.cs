using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public DeployContract deployContract;
    [ContextMenu("DeplyContract")]
    public void DeployContract()
    {
        deployContract.Deploy();
    }
}
