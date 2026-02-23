using UnityEngine;
using UnityEngine.UI; 

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider volumeSlider; 

    void Start()
    {
        
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    
    public void SetVolume(float value)
    {
        AudioListener.volume = value; 
        PlayerPrefs.SetFloat("MasterVolume", value); 
        PlayerPrefs.Save();
    }
}