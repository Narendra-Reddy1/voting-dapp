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
        _errorTxt.gameObject.SetActive(false);
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

        bool isMatch = Regex.IsMatch(voterId, voterIdPattern);
        _errorTxt.gameObject.SetActive(false);
        if (isMatch)
        {
            Debug.Log("The input string matches the pattern.");
            _submitBtn.interactable = true;
        }
        else
        {
            _errorTxt.gameObject.SetActive(true);
            _submitBtn.interactable = false;
        }
    }


    private async void _OnClickSubmit()
    {
        bool isVoted = await BlockchainStateManager.instance.IsCandidatedVotedAlready(_voterIdField.text);
        if (isVoted)
        {
            _errorTxt.gameObject.SetActive(true);
            _errorTxt.SetText("Already Voted !!");
            return;
        }
        ScreenManager.instance.ChangeScreen(Window.CandidateSelectionPanel);
    }

}
