using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuSettingsUI : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Toggles")]
    [SerializeField] private Toggle uiSoundsToggle;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Value Labels")]
    [SerializeField] private TextMeshProUGUI masterVolumeValueText;
    [SerializeField] private TextMeshProUGUI musicVolumeValueText;
    [SerializeField] private TextMeshProUGUI sfxVolumeValueText;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    private const string UiSoundsKey = "UISoundsEnabled";
    private const string FullscreenKey = "FullscreenEnabled";

    private void Start()
    {
        LoadSettings();
        ApplyAllSettings();
        RefreshLabels();

        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        if (uiSoundsToggle != null)
            uiSoundsToggle.onValueChanged.AddListener(OnUiSoundsToggleChanged);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
    }

    private void LoadSettings()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);

        if (uiSoundsToggle != null)
            uiSoundsToggle.isOn = PlayerPrefs.GetInt(UiSoundsKey, 1) == 1;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
    }

    private void ApplyAllSettings()
    {
        if (masterVolumeSlider != null)
            SetMixerVolume("MasterVolume", masterVolumeSlider.value);

        if (musicVolumeSlider != null)
            SetMixerVolume("MusicVolume", musicVolumeSlider.value);

        if (sfxVolumeSlider != null)
            SetMixerVolume("SFXVolume", sfxVolumeSlider.value);

        if (fullscreenToggle != null)
            Screen.fullScreen = fullscreenToggle.isOn;
    }

    private void RefreshLabels()
    {
        if (masterVolumeValueText != null && masterVolumeSlider != null)
            masterVolumeValueText.text = Mathf.RoundToInt(masterVolumeSlider.value * 100f) + "%";

        if (musicVolumeValueText != null && musicVolumeSlider != null)
            musicVolumeValueText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100f) + "%";

        if (sfxVolumeValueText != null && sfxVolumeSlider != null)
            sfxVolumeValueText.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100f) + "%";
    }

    private void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        SetMixerVolume("MasterVolume", value);
        RefreshLabels();
    }

    private void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        SetMixerVolume("MusicVolume", value);
        RefreshLabels();
    }

    private void OnSfxVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(SfxVolumeKey, value);
        SetMixerVolume("SFXVolume", value);
        RefreshLabels();
    }

    private void OnUiSoundsToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(UiSoundsKey, isOn ? 1 : 0);
    }

    private void OnFullscreenToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(FullscreenKey, isOn ? 1 : 0);
        Screen.fullScreen = isOn;
    }

    private void SetMixerVolume(string parameter, float value)
    {
        if (audioMixer == null)
            return;

        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, db);
    }
}