using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Start()
    {
        ScreenManager.instance.ChangeScreen(Window.VoterIdPanel);
        BlockchainStateManager.instance.SetupVotingService();
        GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(false);
    }
}
