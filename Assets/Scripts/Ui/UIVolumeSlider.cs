using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string audioParamater;

    [SerializeField] private float multiplier = 25;

    private void Awake()
    {
        SetupSlider();
    }

    public void SetupSlider()
    {
        slider.onValueChanged.AddListener(SliderValue);
        slider.minValue = .001f;
        slider.value = PlayerPrefs.GetFloat(audioParamater, slider.value);
    }

    private void SliderValue(float value)
    {
        audioMixer.SetFloat(audioParamater, Mathf.Log10(value) * multiplier);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(audioParamater, slider.value);
    }
}
