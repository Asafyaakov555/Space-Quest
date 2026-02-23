using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backGroundMusic : MonoBehaviour
{
    public static backGroundMusic Instance;
    public AudioSource musicSource; 
    public AudioClip gangamStyle;  
    public AudioClip lala;  
    public AudioClip godSound;
    public AudioClip backGroundMusicLevel1;
    public AudioClip backGroundMusicLevel2;
    public AudioClip gameOver;
    [SerializeField] float MusicVolume = 0.4f;
    private const string music_PREF_KEY = "backGroundMusic_Enabled";
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
        LoadState();
    }
    public void PlayBackGround_level1()
    {
        StopAllCoroutines();
        AudioListener.pause = false;
        PlayMusicClip(backGroundMusicLevel1, MusicVolume);
        musicSource.loop=true;

    }
    public void PlayBackGround_level2()
    {
        StopAllCoroutines();
        PlayMusicClip(backGroundMusicLevel2, MusicVolume);

    }
    public void PlayGangamStyle()
    {
        
        if (musicSource.isPlaying && musicSource.clip == gangamStyle) return;

        PlayMusicClip(gangamStyle, MusicVolume);
    }
    public void PlayLala()
    {
        if (musicSource.isPlaying && musicSource.clip == lala) return;

        PlayMusicClip(lala, MusicVolume);
    }
    public void gameoverSound()
    {
        StartCoroutine(GameOverSequence());
    }
    public IEnumerator  GameOverSequence()
    {
        musicSource.Stop();
        musicSource.loop=false;
        yield return new WaitForSeconds(2f);
        PlayMusicClip(gameOver, MusicVolume);
    }


    public void PlayGodSound()
    {
        StartCoroutine(godSoundSequance());
        
    }
    public IEnumerator  godSoundSequance()
    {
        PlayMusicClip(godSound, MusicVolume);
        yield return new WaitForSeconds(10f);// wait untill god mode finish 
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        if(sceneIndex==0) //if we still in level 1
        {
           PlayBackGround_level1();

        }
       
    }

    public void StopSpecialMusic()
    {
        
        if (musicSource.isPlaying && (musicSource.clip == gangamStyle || musicSource.clip == lala ))
        {
            musicSource.Stop();
            musicSource.clip = null; 
            Scene currentScene = SceneManager.GetActiveScene();
            int sceneIndex = currentScene.buildIndex;
            if(sceneIndex==0)
            {
                PlayBackGround_level1();
            }
            else
            {
                PlayBackGround_level2();
            }
            
            
            
        }
    }
    public void stopMusic()
    {
        musicSource.Stop();
    }
    private void PlayMusicClip(AudioClip clip, float volume)
    {
        musicSource.pitch = 1f; 
        musicSource.volume = volume;
        musicSource.clip = clip;
        musicSource.Play();
    }
     public void mute()
    {
        musicSource.mute = true;
        PlayerPrefs.SetInt(music_PREF_KEY, 0); 
        PlayerPrefs.Save();

    }
    public void unmute()
    {
        musicSource.mute = false;
        PlayerPrefs.SetInt(music_PREF_KEY, 1);
        PlayerPrefs.Save();
    }
    public bool IsMuted()
    {
        return musicSource.mute;
    }
    void LoadState()
    {
        int isEnabled = PlayerPrefs.GetInt(music_PREF_KEY, 1);
        musicSource.mute = (isEnabled == 0);
    }

    
}
