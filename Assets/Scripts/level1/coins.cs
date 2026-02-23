using Unity.VisualScripting;
using UnityEngine;

public class coins : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int scoreValue = 10; 
    //the real value is in the inspector of every coin there is differant value 
    [SerializeField] private float rotationSpeed = 300f;
    public AudioClip coinCollectSound;
    [SerializeField]  float soundVolume = 2.5f;
   

    /*

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    */

    private void Update()
    {
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            CollectCoin();
            //PlaySound();
        }
    }

    private void CollectCoin()
    {
        
        if (GameManager.Instance != null)
        GameManager.Instance.IncreaseScore(scoreValue);
        if (sfxSound.Instance != null)
        sfxSound.Instance.PlaySound(coinCollectSound, soundVolume);
        Destroy(gameObject);
  
    }
    
        
    
}