using UnityEngine;
using UnityEngine.Audio;

public class sfxSound : MonoBehaviour
{
    [SerializeField] AudioSource effectsSource;
    public static sfxSound Instance;
    [SerializeField] AudioMixer mainMixer;
    private const string SFX_PREF_KEY = "SFX_Enabled";
    void Awake()
    {
        
        if (Instance != null)
        {
            
            Destroy(gameObject); 
        }
        else
        {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    void Start()
    {
        LoadState();
    }
    void LoadState()
    {
        int isEnabled = PlayerPrefs.GetInt(SFX_PREF_KEY, 1);
        if (isEnabled == 1) unmute();
        else mute();
    }
    public void PlaySound(AudioClip clip, float volume,float pitch = 1f)
    {
        if (clip != null)
        {
        
            effectsSource.PlayOneShot(clip, volume);
        }
    }
    public void mute()
    {
        mainMixer.SetFloat("SFXVol", -80f); 
        PlayerPrefs.SetInt(SFX_PREF_KEY, 0); 
        PlayerPrefs.Save();

    }
    public void unmute()
    {
        mainMixer.SetFloat("SFXVol", 0f);
        PlayerPrefs.SetInt(SFX_PREF_KEY, 1);
        PlayerPrefs.Save();
    }
    public bool IsMuted()
    {
        float currentVol;
        mainMixer.GetFloat("SFXVol", out currentVol);
        return currentVol <= -80f;
    }
}
