using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class appleScript : MonoBehaviour
{
    [SerializeField] float rotationspeed = 200f;

    public AudioClip appleCrunchSound;
    [SerializeField] float soundVolume=1;
    
    /*
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if(gameManager==null)
        Debug.Log("game manager =null!!!");
        
    }
    */


    const string playerstring = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerstring)
        {
            if (sfxSound.Instance != null)
            {
                sfxSound.Instance.PlaySound(appleCrunchSound, soundVolume);
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.appleCollected();
            }
            else
            {
                Debug.LogError("GameManager Instance is missing!");
            }
            //gameManager.appleCollected();
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.Rotate(0f, rotationspeed * Time.deltaTime, 0f);
    }
}
