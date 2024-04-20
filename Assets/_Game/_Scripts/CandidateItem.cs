using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CandidateItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _partySymbol;
    [SerializeField] private Image _partyImg;
    [SerializeField] private Button _button;
    public CandidateData candidateData;

    private void OnEnable()
    {
        if (!_button) TryGetComponent(out _button);
        _button.onClick.AddListener(_OnSelect);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(_OnSelect);
    }

    private void _OnSelect()
    {
        GlobalEventHandler.OnCandidateSelectedToVote?.Invoke(this);
    }

    public void Setup(CandidateData candidate, Sprite partyImg)
    {
        candidateData = candidate;
        _partySymbol.SetText(candidate.partySymbol);
        _partyImg.sprite = partyImg;
    }


}
