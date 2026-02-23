using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseGameManager
{
    public static GameManager Instance;

    [Header("Game Objects")]
    [SerializeField] GameObject GodModeVisual;
    [SerializeField] CinemachineCamera cin;
    [SerializeField] GameObject Monster;

    public  GameObject activeMonster;
    //[SerializeField] GameObject player;
    [SerializeField] Level_Genarator level_Genarator;
    public PlayerMovement playerMovement;
    //[SerializeField] UIManager uIManager;
    [Header("settings")]
    [SerializeField] float scoreToNextLevel=500;
    int maxApples=3;
    int currentApples = 0;

    public bool GodMode=false;
    //public bool GameStarted=false;
    //public bool isPaused=false;
    int score=0;
    [Header("Settings")]
    [SerializeField] float godModeDuration = 10f;
    [SerializeField] float monsterRespawnDelay = 2f;
   
    void Awake()
    {
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
   
    protected override void Update()
    {
        if(GodMode && Input.GetKeyDown(KeyCode.G))
        {
            level_Genarator.GodMode=true;
            activateGodMode();
        }
        /*
        if (GameStarted)
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    uIManager.ResumeGame();
                else
                    uIManager.PauseGame();
            }
            
        }
        */
        //MonsterFall();
        base.Update();
        
    }
    public void deathRoadLoad()
    {
        if (backGroundMusic.Instance != null)
        {
            backGroundMusic.Instance.PlayBackGround_level2();
        }
        SceneManager.LoadScene(1);  
    }
    public void activateGodMode()
    {
        
        if(playerMovement.transform.position.y>=0.8)
        {
            uIManager.turnOnGodModeWarning();
            return;
        }
        
        uIManager.stopBlinking();
        GodModeVisual.SetActive(true);
        Invoke("resetGODmode",godModeDuration);
        if (activeMonster != null)
        {
            Enemy enemyScript = activeMonster.GetComponent<Enemy>();
            if (enemyScript != null) enemyScript.GodModeEnabled();
        }
        /*
        Enemy enemy = FindAnyObjectByType<Enemy>();
        if(enemy!=null)
        enemy.GodModeEnabled();
        */
        playerMovement.GodMode();
        level_Genarator.DestroyAllItems();
        
        
    }
    public void appleCollected()
    {
        currentApples++;
        float progress = (float)currentApples / maxApples;
        uIManager.updateAppleImage(progress,maxApples,currentApples); 
    }
    void resetGODmode()
    {
        uIManager.resetAppleImage();
        currentApples=0;
        level_Genarator.GodMode=false;
        GodModeVisual.SetActive(false);
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
        uIManager.updateScore(score);
        if (score >= scoreToNextLevel)
        {
            deathRoadLoad();// load the next level
        }
           
    }
    bool lose_Bool=false;
    
    public override void lose() 
    {
        lose_Bool=true;
        /*
        Enemy enemy = FindAnyObjectByType<Enemy>();//לשפר עם המפלצת הזמינה 
        if(enemy!=null)
        {
            enemy.EnemyVICTORY();
            enemy.turnOffANIMATE();
        }
        */
        if(activeMonster!=null)
        {
            Enemy enemy = activeMonster.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyVICTORY();
                enemy.turnOffANIMATE();
            }
        }
        cin.Target.TrackingTarget = activeMonster.transform;
        level_Genarator.loseBool=true;
        base.lose();
        
    }
    public void  createMonster()
    {
        
        if(lose_Bool)
        return;
        Vector3 PlayerPos = player.transform.position;
        Vector3 spawnPos = PlayerPos - (player.transform.forward * 4f);
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, 20.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
                    
            activeMonster = Instantiate(Monster, hit.position, Quaternion.identity);
            activeMonster.transform.LookAt(PlayerPos);
            
        }
                         
    }
    public void  MonsterFall()
    {
        /*
        if (activeMonster != null) 
        {
            if (activeMonster.transform.position.y <= monsterFallYThreshold)
            {
                Destroy(activeMonster); 
                Invoke("createMonster", monsterRespawnDelay);
            }
        }
        */
        Destroy(activeMonster); 
        Invoke("createMonster", monsterRespawnDelay);
    }
    public void  startGame()
    {
        
        GameStarted=true;
        level_Genarator.SpawnStartingFloor();
        GodModeVisual.SetActive(false);
        player.SetActive(true);
        backGroundMusic.Instance.PlayBackGround_level1();
        createMonster();
        
    }
}
