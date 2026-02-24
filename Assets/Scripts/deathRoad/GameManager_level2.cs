using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager_level2 : BaseGameManager
{
    public static GameManager_level2 Instance;
    public PlayerMovement playerMovement;
   
    [Header("Texts")]
    [SerializeField] TMP_Text Win;
    [SerializeField] TMP_Text loser;
    
    [Header("Game Objects")]
    [SerializeField] ParticleSystem victoryFireworks;
    [SerializeField] VideoPlayer videoPlayer;
    public VideoClip[] movieLibrary; 
    [SerializeField] GameObject restartW;
    
    bool win=false;
    public float delayStartVideo=3f;
    
   
    void Awake()
    {
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        GameStarted = true;
    }
    public override void lose()
    {
        base.lose();
    }
    public void Winner()
    {
        if(win)return;
        backGroundMusic.Instance.stopMusic();
        //PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if(playerMovement!=null)
        playerMovement.win();
        Win.gameObject.SetActive(true);
        Transform cameraTransform = Camera.main.transform; // prepare the camare
        Vector3 spawnPos = cameraTransform.position + (cameraTransform.forward * 40f);
        Vector3 directionToCamera = cameraTransform.position - spawnPos;
        Quaternion rot = Quaternion.LookRotation(directionToCamera);
        ParticleSystem newFireworks = Instantiate(victoryFireworks, spawnPos, rot);
        newFireworks.Play();
        win=true;
        Invoke("startVi",delayStartVideo);
        
        
    }
    
    void startVi()
    {
        
        Win.gameObject.SetActive(false);
        int skyIndex=SkyboxManager.Instance.getSkyIndex();// to match the currect video to the sky
        if (skyIndex >= 0 && skyIndex < movieLibrary.Length)//for every sky different video 
        {
            videoPlayer.clip = movieLibrary[skyIndex];
            videoPlayer.Play();
        }
        restartW.SetActive(true);
    }
    
    
    
    
    
}
