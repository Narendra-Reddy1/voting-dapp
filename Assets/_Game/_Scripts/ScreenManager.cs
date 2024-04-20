using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//with addressables can be optimized it more.
//destroying and instantiating will cause garbage collection frequently.
//ut for this example it's fine.
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<Window, GameObject> m_screens;

        [SerializeField] private Dictionary<Window, GameObject> m_dynamicScreens = new Dictionary<Window, GameObject>();
        private Stack<Window> m_screenStack = new Stack<Window>();
        private Window m_currentScreen = Window.None;
        private Window m_previousScreen = Window.None;

        public static ScreenManager instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }


        public GameObject ChangeScreen(Window window, ScreenType screenType = ScreenType.Replace)
        {
            //if (m_currentScreen == window) { return null; }
            if (!m_screens.ContainsKey(window)) return null;
            if (screenType == ScreenType.Replace)
            {
                CloseAllScreens();
            }
            GameObject screen = m_screens[window];
            GameObject spawnedScreen = Instantiate(screen, transform);
            spawnedScreen.transform.SetAsLastSibling();
            if (!m_dynamicScreens.ContainsKey(window))
            {
                m_dynamicScreens.Add(window, spawnedScreen);
            }
            else
                m_dynamicScreens[window] = spawnedScreen;
            m_previousScreen = m_currentScreen;
            m_currentScreen = window;
            if (screenType == ScreenType.Additive)
                m_screenStack.Push(window);
            return spawnedScreen;
        }
        public void CloseScreen(Window window)
        {
            if (m_dynamicScreens.ContainsKey(window))
            {
                GameObject go;
                if (m_dynamicScreens.TryGetValue(window, out go))
                {
                    Destroy(go);
                    m_dynamicScreens.Remove(window);

                }
            }
        }
        public void CloseAllScreens()
        {
            foreach (GameObject go in m_dynamicScreens.Values)
            {
                Destroy(go);
            }
            m_dynamicScreens.Clear();
            m_screenStack.Clear();
        }
    }

    public enum Window
    {
        None,
      VoterIdPanel,
      StartNewElectionPanel,
      CandidateSelectionPanel,
      GenericElectionPopup,


      //AddCandidatePanel,

    }
    public enum ScreenType
    {
        Additive,
        Replace,
    }