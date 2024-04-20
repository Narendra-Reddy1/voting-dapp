using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    [SerializeField] private Window _window;
    [SerializeField] private ScreenType _screenType;

    public void OpenWindow()
    {
        ScreenManager.instance.ChangeScreen(_window, _screenType);
    }
}
