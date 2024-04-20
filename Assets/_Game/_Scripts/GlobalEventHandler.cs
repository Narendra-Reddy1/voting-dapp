using System;
using System.Collections;
using UnityEngine;


public static class GlobalEventHandler
{
    public static Action<bool> OnLoadingPanelToggleRequested = default;
    public static Action<CandidateItem> OnCandidateSelectedToVote = default;


}