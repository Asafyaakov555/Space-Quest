using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance;

    public GameObject menu;
    [Header("images")]
    [SerializeField] Image AppleImage;
    [SerializeField] Image musicButton;
    [SerializeField] Image sfxButton;
    public Sprite imageOn;
    public Sprite imageOff;


    [SerializeField] RawImage instructionsImage;
    [Header("buttons")]
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playResume;
    [SerializeField] GameObject skyBoxChoose;
    [SerializeField] GameObject howToPlayButton;
    [SerializeField] GameObject settingsScreeN;
    [SerializeField] GameObject firstButtons;//the 3 started buttons play,how to play,settings
    [SerializeField] GameObject settingsForPause;
    [SerializeField] GameObject restartW;
    [Header("Texts")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text loser;
    [SerializeField] TMP_Text godModeWarning;
    /*
    [Header("Game Objects")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameManager_level2 gameManager_Level2;
    */
    [Header("Manager Connection")]
    [SerializeField] BaseGameManager activeGameManager;
    [Header("settings")]
    Color darkColor = new Color(0.3f, 0.3f, 0.3f, 1f); 
    Color brightColor = Color.white;
    Coroutine blinkRoutine;
    
    public bool isPaused = false;
    bool isBlinking=false;
    bool showInstructions=false;
    // bool variable to know the state of music and sfx ** started as true
    bool musicSound=true;
    bool sfxSoundBool=true;
    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        UpdateSettingsIcons();
    }
    public void UpdateSettingsIcons()
    {
        
        if (sfxSound.Instance != null)
        {
            
            bool isMuted = sfxSound.Instance.IsMuted(); 
            sfxSoundBool = !isMuted; 
            sfxButton.sprite = sfxSoundBool ? imageOn : imageOff;
        }
        
        if (backGroundMusic.Instance != null)
        {
            
            bool isMuted = backGroundMusic.Instance.IsMuted(); 
            musicSound = !isMuted; 
            musicButton.sprite = musicSound ? imageOn : imageOff;
        }
    }

    //level 1


    public void stopBlinking()
    {
        isBlinking = false;
        StopCoroutine(blinkRoutine);
        StartCoroutine(DrainAppleBar(10f));
        
    }
    public void turnOnGodModeWarning()
    {
        godModeWarning.gameObject.SetActive(true);
        Invoke("turnOffWarning",1f);
    }
    

    void turnOffWarning()
    {
        godModeWarning.gameObject.SetActive(false);
    }
    IEnumerator DrainAppleBar(float duration)
    {
        float timer = 0f;
        AppleImage.color = Color.white; 
        while (timer < duration)
        {
            timer += Time.deltaTime;
            
            
            float fill = 1f - (timer / duration);
            AppleImage.fillAmount = fill;

            yield return null; 
        }
        AppleImage.fillAmount = 0f;
    }
    public void resetAppleImage()
    {
        AppleImage.fillAmount = 0f;
    }
    
    IEnumerator BlinkApple()
    {
        

        while (isBlinking)
        {
            AppleImage.color = brightColor;
            yield return new WaitForSeconds(0.3f);

            AppleImage.color = darkColor;
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void updateScore(int amount)
    {
        scoreText.text = amount.ToString();
           
    }
    public void updateAppleImage(float progress,int maxApples,int currentApples)
    {
         
        AppleImage.fillAmount = progress;
        if(currentApples>0 && currentApples % maxApples==0)
        {
            isBlinking=true;
            blinkRoutine= StartCoroutine(BlinkApple());
            //gameManager.GodMode=true;
            ((GameManager)activeGameManager).GodMode = true;

            
            
        }
    }
    // level1 + level2
    
    public void lose()
    {
        
        loser.gameObject.SetActive(true);
        pauseMenu.SetActive(true);
        playResume.SetActive(false);
        settingsForPause.SetActive(false);
        if(AppleImage != null) AppleImage.gameObject.SetActive(false);
        /*
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        if(sceneIndex==0)
        AppleImage.gameObject.SetActive(false);
        */
    }
    public void startGame()
    {
        GameManager.Instance.startGame();
        scoreText.gameObject.SetActive(true);
        AppleImage.gameObject.SetActive(true);
        AppleImage.fillAmount = 0f;
        firstButtons.SetActive(false);
    }
    
    
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused=true;
        /*
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        //Debug.Log("current index"+sceneIndex);
        if(sceneIndex==0) //level1
        {
            //gameManager.isPaused = true;
            

        }
        if(sceneIndex==1)//level2
        {
            //gameManager_Level2.isPaused=true;
            
            
        }
        */
        if(activeGameManager != null)
        {
            activeGameManager.isPaused = true;
        }

        
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused=false;
        /*
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        if(sceneIndex==0) //level1
        {
            gameManager.isPaused = false;

        }
        if(sceneIndex==1)//level2
        {
            gameManager_Level2.isPaused=false;
        }
        */
        if(activeGameManager != null)
        {
            activeGameManager.isPaused = false;
        }
        

    }
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        //Debug.Log("current index"+sceneIndex);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        //צריך להוסיף אזהרה ולעשות restart לשלב 1
    }
    
    public void instructions()
    {
        showInstructions=!showInstructions;
        if(showInstructions)
        {
            firstButtons.SetActive(false);
            instructionsImage.gameObject.SetActive(true);
            return;

        }
        if(!showInstructions)
        {
            firstButtons.SetActive(true);
            instructionsImage.gameObject.SetActive(false);
            
            return;

        }

        
    }
    public void settingsScreen()
    {
        
        if(!isPaused)
        {
            /*
            Scene currentScene = SceneManager.GetActiveScene();
            int sceneIndex = currentScene.buildIndex;
            if(sceneIndex==0)
            firstButtons.SetActive(false);
            if(sceneIndex==1)
            pauseMenu.SetActive(false);
            settingsScreeN.SetActive(true);
            */
            firstButtons.SetActive(false);
            settingsScreeN.SetActive(true);
            
        }
        else
        {
           settingsScreeN.SetActive(true);
           pauseMenu.SetActive(false);
           
            
        }
        
        
    }
    public void applySetting()
    {
        
        
        if(!isPaused)
        {
            settingsScreeN.SetActive(false);
            /*
            Scene currentScene = SceneManager.GetActiveScene();
            int sceneIndex = currentScene.buildIndex;
            if(sceneIndex==0)
            firstButtons.SetActive(true);
            */
            if(firstButtons!=null)
            firstButtons.SetActive(true);

            
            

        }
        else
        {
            settingsScreeN.SetActive(false);
            pauseMenu.SetActive(true);
        }
        
    }
    public void sfxClicked()
    {
        if(sfxSoundBool)
        sfxTurnoff();
        else
        sfxTurnOn();
        
        

    }
    public void musicClicked()
    {
        if(musicSound)
        musicTurnoff();
        else
        musicTurnOn();
    }
    void sfxTurnoff()
    {
        sfxSoundBool=false;
        sfxButton.sprite=imageOff;
        sfxSound.Instance.mute();
    }
    void sfxTurnOn()
    {
        sfxSoundBool=true;
        sfxButton.sprite=imageOn;
        sfxSound.Instance.unmute();
    }
    void musicTurnoff()
    {
        musicSound=false;
        musicButton.sprite=imageOff;
        backGroundMusic.Instance.mute();
    }
    void musicTurnOn()
    {
        musicSound=true;
        musicButton.sprite=imageOn;
        backGroundMusic.Instance.unmute();
    }
    
    public void exitGame()
    {
        GameManager.Instance.QuitGame();
        // In the latest version of the game, I disabled the exit button because 
        // I changed the build profile from fullscreen to windowed.
        // This option provides an automatic exit button which looks better.
        // I didn't delete the button permanently, to serve as a backup for non-windowed builds.
    }
    

    
}