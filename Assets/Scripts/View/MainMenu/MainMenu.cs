using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    
    [SerializeField]
    private MenuPanel mainPanel, startPanel, howToPlayPanel, optionsPanel;

    private MenuPanel activePanel, lastPanel;

    [SerializeField]
    private OptionsMenu _optionsMenu;
    public static OptionsMenu optionsMenu { get; private set; }

    [SerializeField]
    private MenuPanel _pausedOverlay;
    public static MenuPanel pausedOverlay { get; private set; }

    /*[SerializeField]
    private GameObject _muteImage, _unmuteImage, _fullscreenImage, _windowedImage;
    public static GameObject muteImage { get; private set; }
    public static GameObject unmuteImage { get; private set; }
    public static GameObject fullscreenImage { get; private set; }
    public static GameObject windowedImage { get; private set; }*/


    private static MainMenu _mainMenu;
    public static MainMenu mainMenu
    {
        get
        {
            if (_mainMenu == null)
                _mainMenu = FindObjectOfType<MainMenu>();

            return _mainMenu;
        }
    }

    private void Awake()
    {
        activePanel = lastPanel = mainPanel;

        optionsMenu = _optionsMenu;
        pausedOverlay = _pausedOverlay;
        /*gameoverOverlay = _gameoverOverlay;
        upgradeOverlay = _upgradeOverlay;

        muteImage = _muteImage;
        unmuteImage = _unmuteImage;
        fullscreenImage = _fullscreenImage;
        windowedImage = _windowedImage;*/
    }

    public void OnStartGameClick()
    {
        if (startPanel.activeSelf)
        {
            GameController.StartGame();
        }
        else
        {
            ChangeActivePanel(startPanel);
        }
    }

    public void OnHowToPlayClick()
    {
        ChangeActivePanel(howToPlayPanel);
    }

    public void OnOptionsClick()
    {
        ChangeActivePanel(optionsPanel);
    }

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnBackClick()
    {
        ChangeActivePanel(lastPanel);
    }

    public void OnUnpauseClick()
    {
        GameController.PauseGame(false);
    }

    public void OnQuitToMenuClick()
    {
        GameController.EndGame();
    }

    public void OnRetryClick()
    {
        GameController.ClearLevel();

        GameController.StartGame();
    }

    public void OnEnterMainMenu()
    {
        ChangeActivePanel(mainPanel);
    }

    public void OnMuteSoundsClick()
    {
        SetSoundsMuted(!OptionsController.SoundMuted);
    }

    /*public void OnUnmuteSoundsClick()
    {
        SetSoundsMuted(false);
    }*/

    public void OnFullscreenClick()
    {
        SetFullscreen(!Screen.fullScreen);
    }

    /*public void OnWindowedClick()
    {
        SetFullscreen(false);
    }*/

    public void OnInputCancel()
    {
        if (pausedOverlay.activeSelf)
            OnUnpauseClick();
        else if (activePanel.panelObject != mainPanel.panelObject)
            OnBackClick();
    }

    public static void SetPauseOverlayActive(bool active)
    {
        pausedOverlay.SetActive(active);

        if(active)
            MenuSelector.SelectedObject = pausedOverlay.firstPanelObject;
        else
            MenuSelector.SelectedObject = null;
    }

    public static void SetSoundsMuted(bool muted)
    {
        OptionsController.SoundMuted = muted;
    }

    public static void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    private void ChangeActivePanel(MenuPanel panel)
    {

        lastPanel = activePanel;
        activePanel.SetActive(false);

        activePanel = panel;
        activePanel.SetActive(true);
        MenuSelector.SelectedObject = activePanel.firstPanelObject;
    }
}

[Serializable]
public struct MenuPanel {

    public GameObject panelObject;
    public GameObject firstPanelObject;

    public bool activeSelf
    {
        get
        {
            return panelObject.activeSelf;
        }
    }

    public static implicit operator GameObject(MenuPanel menuPanel)
    {
        return menuPanel.panelObject;
    }

    public void SetActive(bool active)
    {
        panelObject.SetActive(active);
    }
}
