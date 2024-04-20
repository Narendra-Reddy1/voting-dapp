using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class VoterIdPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _voterIdField;
    [SerializeField] private Button _submitBtn;
    [SerializeField] private TextMeshProUGUI _errorTxt;
    const string voterIdPattern = @"^[a-zA-Z]{3}\d{7}$";

    private void OnEnable()
    {
        _submitBtn.interactable = false;
        _voterIdField.onEndEdit.AddListener(_ValidateVoterId);
        _submitBtn.onClick.AddListener(_OnClickSubmit);
    }
    private void OnDisable()
    {
        _voterIdField.onEndEdit.RemoveListener(_ValidateVoterId);
        _submitBtn.onClick.RemoveListener(_OnClickSubmit);
    }

    private void _ValidateVoterId(string voterId)
    {

        _errorTxt.SetText(string.Empty);
        if (Regex.IsMatch(voterId, voterIdPattern) && _voterIdField.text.Length == Konstants.LENGTH_OF_VOTER_ID)
        {
            _submitBtn.interactable = true;
        }
        else
        {

            _errorTxt.SetText("Incorrect Voter ID :(");
            _submitBtn.interactable = false;
        }
    }


    private async void _OnClickSubmit()
    {
        GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(true);
        bool isVoted = await BlockchainStateManager.instance.IsCandidatedVotedAlready(_voterIdField.text);
        if (isVoted)
        {
            _errorTxt.SetText("Already Voted !!");
            return;
        }
        GlobalVariables.voterID = _voterIdField.text;
        ScreenManager.instance.ChangeScreen(Window.CandidateSelectionPanel);
    }

}
