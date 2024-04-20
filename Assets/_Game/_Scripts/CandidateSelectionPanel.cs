using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CandidateSelectionPanel : MonoBehaviour
{
    [SerializeField] private string candidateDataPath;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private CandidateItem _candidateItem;
    [SerializeField] private Sprite defaultImg;
    [SerializeField] private Button _submitBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private Transform _selectedOverlay;

    private CandidateList _candidateList;

    private CandidateItem _selectedCandidate;

    private void OnEnable()
    {
        GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(true);
        _cancelBtn.onClick.AddListener(_OnCancel);
        _submitBtn.onClick.AddListener(_OnSubmit);
        GlobalEventHandler.OnCandidateSelectedToVote += Callback_On_Candiate_Selected;
        SetupPanel();
    }
    private void OnDestroy()
    {
        _submitBtn.onClick.RemoveListener(_OnSubmit);
        _cancelBtn.onClick.RemoveListener(_OnCancel);
        GlobalEventHandler.OnCandidateSelectedToVote -= Callback_On_Candiate_Selected;
    }


    public async void SetupPanel()
    {
        using (System.IO.StreamReader reader = new System.IO.StreamReader(candidateDataPath))
        {
            _candidateList = JsonUtility.FromJson<CandidateList>(reader.ReadToEnd());
        }
        foreach (CandidateData candidate in _candidateList.candidates)
        {
            var item = Instantiate(_candidateItem, _contentParent);
            Sprite partyImg = await DownloadImage(candidate.imageUrl);
            item.Setup(candidate, partyImg);
        }
        GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(false);
    }

    private void _UpdateSelectedCandidate()
    {
        _selectedOverlay.SetAsLastSibling();
        _selectedOverlay.gameObject.SetActive(true);
        _selectedOverlay.transform.position = _selectedCandidate.transform.position;
    }

    private void Callback_On_Candiate_Selected(CandidateItem selectedCandidate)
    {
        if (_selectedCandidate == selectedCandidate) return;
        _selectedCandidate = selectedCandidate;
        _UpdateSelectedCandidate();
    }

    private void _OnSubmit()
    {

        GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(true);
        BlockchainStateManager.instance.RecordVote(_selectedCandidate.candidateData.id, GlobalVariables.voterID, () =>
        {
            GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(false);
            GenericPopup popup = ScreenManager.instance.ChangeScreen(Window.GenericElectionPopup, ScreenType.Additive).GetComponent<GenericPopup>();
            popup.SetupPopup(PopupType.Success, $"Voted to {_selectedCandidate.candidateData.partySymbol}", () =>
             {
                 ScreenManager.instance.ChangeScreen(Window.VoterIdPanel);
             });
        }, (revetMsg) =>
         {
             GenericPopup popup = ScreenManager.instance.ChangeScreen(Window.GenericElectionPopup, ScreenType.Additive).GetComponent<GenericPopup>();
             popup.SetupPopup(PopupType.Error, revetMsg, () =>
             {
                 ScreenManager.instance.ChangeScreen(Window.VoterIdPanel);
             });
             GlobalEventHandler.OnLoadingPanelToggleRequested?.Invoke(false);
         });
    }

    private void _OnCancel()
    {
        ScreenManager.instance.ChangeScreen(Window.VoterIdPanel);
    }

    private async Task<Sprite> DownloadImage(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    byte[] imageData = await response.Content.ReadAsByteArrayAsync();
                    //string filePath = System.IO.Path.Combine(Application.persistentDataPath, "downloadedImage.jpg");
                    //System.IO.File.WriteAllBytes(filePath, imageData);
                    //Debug.Log($"Image downloaded and saved to: {filePath}");
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);
                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                }
            }
            return defaultImg;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return defaultImg;
        }
    }


    private class CandidateList
    {
        public List<CandidateData> candidates;

    }
}


[System.Serializable]
public class CandidateData
{
    public string imageUrl;
    public string id;
    public string partySymbol;
}