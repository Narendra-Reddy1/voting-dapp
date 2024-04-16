using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Nethereum.Web3;

public class VoterIdValidator : MonoBehaviour
{
    #region Variables

    [SerializeField] private TMP_InputField _voterIdField;
    [SerializeField] private Button _submitBtn;
    [SerializeField] private TextMeshProUGUI _invalidVoterIdTxt;
    [SerializeField]private GameObject _loadingPanel;
    //private string pattern = "^[a-zA-Z]{3}";
    private string pattern = @"^[a-zA-Z]{3}\d{7}$";

    private const byte LENGTH_OF_VOTER_ID = 10;

    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        _submitBtn.interactable = false;
        _submitBtn.onClick.AddListener(_OnClickSubmit);
        _voterIdField.onEndEdit.AddListener(_ValidateVoterID);
    }
    private void OnDisable()
    {
        _submitBtn.onClick.RemoveListener(_OnClickSubmit);
        _voterIdField.onEndEdit.RemoveListener(_ValidateVoterID);

    }
    #endregion Unity Methods


    #region Private Methods
    private void _OnClickSubmit()
    {
        _ToggleLoadingPanel(true);
     
    }
    private void _ValidateVoterID(string value)
    {
        if (Regex.IsMatch(_voterIdField.text, pattern) && _voterIdField.text.Length == LENGTH_OF_VOTER_ID)
        {
            _invalidVoterIdTxt.SetText(string.Empty);
            _submitBtn.interactable = true;
        }
        else
        {
            _invalidVoterIdTxt.SetText($"Invalid Voter ID");
            _submitBtn.interactable = false;
        }
    }

    private void _ToggleLoadingPanel(bool value)
    {
        if (_loadingPanel)
            _loadingPanel.SetActive(value);
    }
    #endregion Private Methods

    #region Public  Methods
    #endregion Public Methods

    #region Callbacks
    #endregion Callbacks

}
