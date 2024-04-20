using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PopupType
{
    Success,
    Info,
    Error,
}

public class GenericPopup : MonoBehaviour
{

    [SerializeField] private SerializedDictionary<PopupType, Sprite> _imageDict;
    [SerializeField] private TextMeshProUGUI _messageTxt;
    [SerializeField] private Button _actionBtn;
    [SerializeField] private Image _popupImg;



    public void SetupPopup(PopupType popupType, string message, UnityAction action = null)
    {
        _actionBtn.onClick.RemoveAllListeners();
        _actionBtn.onClick.AddListener(action != null ? action : () => { gameObject.SetActive(false); });
        _messageTxt.SetText(message);
    }

}
