using UnityEngine;

public class BaseGameManager : MonoBehaviour
{
    
    [Header("Base Settings")]
    public bool isPaused = false;
    public bool GameStarted = false;

    [SerializeField] protected UIManager uIManager;
    [SerializeField] protected GameObject player;

    protected virtual void Update()
    {
        if (GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseLogic();
            }
        }
    }

    public void TogglePauseLogic()
    {
        if (isPaused)
            uIManager.ResumeGame();
        else
            uIManager.PauseGame();
    }

    public virtual void lose()
    {
        backGroundMusic.Instance.gameoverSound();
        uIManager.lose();
        Debug.Log("asaf");
    }
    public void QuitGame()
    {
        //Debug.Log("Quit Game Request");
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}