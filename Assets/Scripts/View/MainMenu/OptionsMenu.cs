using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    
    [SerializeField]
    private Text sfxVolumeLabel, musicVolumeLabel;
    public Slider sfxVolumeSlider, musicVolumeSlider;
    public Toggle showScoreToggle, fullscreenToggle, colorBlindToggle;
    public Dropdown qualityDropdown;

    private static OptionsMenu _optionsMenu;
    public static OptionsMenu optionsMenu
    {
        get
        {
            if (_optionsMenu == null)
                _optionsMenu = FindObjectOfType<OptionsMenu>();

            return _optionsMenu;
        }
    }

    public void OnOptionsLoaded()
    {
        musicVolumeLabel.text = "Music Volume: " + (int)(OptionsController.MusicVolume * 100) + "%";
        musicVolumeSlider.value = OptionsController.MusicVolume;
        sfxVolumeLabel.text = "SFX Volume: " + (int)(OptionsController.SFXVolume * 100) + "%";
        sfxVolumeSlider.value = OptionsController.SFXVolume;
        showScoreToggle.isOn = OptionsController.ShowScore;
        fullscreenToggle.isOn = OptionsController.Fullscreen;
        colorBlindToggle.isOn = OptionsController.ColorBlind;
        qualityDropdown.value = OptionsController.Quality;
    }

    public void OnMusicVolumeBarChanged(float value)
    {
        musicVolumeLabel.text = "Music Volume: " + (int)(value * 100) + "%";

        OptionsController.MusicVolume = value;
    }

    public void OnSFXVolumeBarChanged(float value)
    {
        sfxVolumeLabel.text = "SFX Volume: " + (int)(value * 100) + "%";

        OptionsController.SFXVolume = value;
    }

    public void OnShowScoreToggle(bool toggle)
    {
        OptionsController.ShowScore = toggle;
    }

    public void OnFullscreenToggle(bool toggle)
    {
        OptionsController.Fullscreen = toggle;
    }

    public void OnColorBlindToggle(bool toggle)
    {
        OptionsController.ColorBlind = toggle;
    }

    public void OnQualityChanged(int quality)
    {
        OptionsController.Quality = quality;
    }
}
