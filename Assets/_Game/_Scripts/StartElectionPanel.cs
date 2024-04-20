using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class StartElectionPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _startElectionField;
    [SerializeField] private TMP_InputField _endElectionField;
    [SerializeField] private TMP_InputField _resultElectionField;

    [SerializeField] private Button _submitBtn;
    [SerializeField] private Button _cancelBtn;
    private bool _isStartTimeGiven;
    private bool _isEndTimeGiven;

    private void OnEnable()
    {
        _submitBtn.interactable = false;
        _submitBtn.onClick.AddListener(_OnSubmit);
        _cancelBtn.onClick.AddListener(_OnCancel);
        _startElectionField.onEndEdit.AddListener(_StartElectionTxtChanged);
        _endElectionField.onEndEdit.AddListener(_EndElectionTxtChanged);
    }
    private void OnDisable()
    {
        _submitBtn.onClick.RemoveListener(_OnSubmit);
        _cancelBtn.onClick.RemoveListener(_OnCancel);
    }

    private void _StartElectionTxtChanged(string val)
    {
        _isStartTimeGiven = !(string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val));
        _submitBtn.interactable = (_isStartTimeGiven && _isEndTimeGiven);
    }
    private void _EndElectionTxtChanged(string val)
    {
        _isEndTimeGiven = !(string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val));
        _submitBtn.interactable = (_isStartTimeGiven && _isEndTimeGiven);
    }
    private void _OnSubmit()
    {
        //Trigger loading Screen
        BigInteger startTime = BigInteger.Parse(_startElectionField.text);
        BigInteger endTime = BigInteger.Parse(_endElectionField.text);
        BigInteger resultTime = 0;
        if (!(string.IsNullOrEmpty(_resultElectionField.text) && string.IsNullOrWhiteSpace(_resultElectionField.text)))
            resultTime = BigInteger.Parse(_resultElectionField.text);
        else
            resultTime = endTime;
        if (IsElectionTimingIsValid(startTime, endTime, resultTime))
        {
            BlockchainStateManager.instance.StartElection(startTime, endTime, resultTime, () =>
            {
                //CLose loading Screen
                GenericPopup popup = ScreenManager.instance.ChangeScreen(Window.GenericElectionPopup, ScreenType.Additive).GetComponent<GenericPopup>();
                popup.SetupPopup(PopupType.Success, "Election Timings Updated Successfully!!!", () =>
                {
                    //where to go??
                    //Voter id Panel*
                });

            }, (revetMsg) =>
            {
                //Close LoadingScreen
                GenericPopup popup = ScreenManager.instance.ChangeScreen(Window.GenericElectionPopup, ScreenType.Additive).GetComponent<GenericPopup>();
                popup.SetupPopup(PopupType.Error, "Something Went Wrong!!!");
            });
        }
        else
        {
            //Close LoadingScreen
            GenericPopup popup = ScreenManager.instance.ChangeScreen(Window.GenericElectionPopup, ScreenType.Additive).GetComponent<GenericPopup>();
            popup.SetupPopup(PopupType.Error, "Incorrect Timing!!!");
        }
    }
    private bool IsElectionTimingIsValid(BigInteger start, BigInteger end, BigInteger result)
    {
        return ((start > 0 && end > 0 && result > 0) && ((start < end) && (end <= result) && (start < result)));
    }


    private void _OnCancel()
    {
        //Go back
    }
}
