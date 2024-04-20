using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelAnimator : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<GameObject> _blocksImgList;

    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        GlobalEventHandler.OnLoadingPanelToggleRequested += (value) =>
        {
            this.gameObject.SetActive(value);
        };
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(_ShowRandomCube), 0, 0.15f);
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(_ShowRandomCube));
    }
    #endregion Unity Methods


    #region Private Methods
    private void _ShowRandomCube()
    {
        _DisableAllCubes();
        _blocksImgList[Random.Range(0, _blocksImgList.Count)].SetActive(true);
    }
    private void _DisableAllCubes()
    {
        _blocksImgList.ForEach(x => x.SetActive(false));
    }
    #endregion Private Methods

    #region Public  Methods
    #endregion Public Methods

    #region Callbacks
    #endregion Callbacks
}
