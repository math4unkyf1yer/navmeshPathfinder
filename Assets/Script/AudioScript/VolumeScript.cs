using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        // Load saved volume if available
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Add listener for when the slider value changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // Convert linear slider value to logarithmic scale for audio
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        // Save volume setting
        PlayerPrefs.SetFloat("Volume", volume);
    }
}