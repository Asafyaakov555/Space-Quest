using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    [SerializeField] GameObject boulder;
    [SerializeField] GameObject pos;
    [SerializeField] GameObject brickParent;
    [SerializeField] GameObject forDestroy;
    bool playerPass=false;
    
    
    [SerializeField] float launchForceMax = 15f;
    [SerializeField] float launchForceMin = 10f;
    [SerializeField] BoxCollider startLaunching;
    [SerializeField] float shakeDuration = 2.0f;
    
    
    [SerializeField] float shakeMagnitude = 0.1f;
    
    bool launching=false;
    GameObject player;
    [Header("Audio Settings")]
    [SerializeField] AudioClip shakeSound; 
    [SerializeField] AudioSource audioSource; 
    [SerializeField] [Range(0f, 1f)] float soundVolume = 1f;


    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }
   
    void OnTriggerEnter(Collider other)
    {
        if(!launching)
        {
            if(other.CompareTag("Player"))
            {
                
                StartCoroutine(SpawnBouldersRoutine());
                launching=true;
                startLaunching.enabled=false;
            }
        }
        
       
    }

    IEnumerator SpawnBouldersRoutine() 
    {
        while (!playerPass)
        {
           
            SpawnSingleBoulder();
            yield return new WaitForSeconds(2f); // if the player not pass create more rocks 
        }
    }

    void SpawnSingleBoulder()
    {
        float randomX = Random.Range(-4.9f, 4.9f);
        Vector3 spawnPosition = new Vector3(randomX, pos.transform.position.y, pos.transform.position.z);
        GameObject newBoulder = Instantiate(boulder, spawnPosition, Quaternion.identity,brickParent.transform);
        Rigidbody rb = newBoulder.GetComponent<Rigidbody>();
        float randomForce = Random.Range(launchForceMin, launchForceMax);
        if (rb != null)
    {
        
        Vector3 targetPosition = player.transform.position;
        float deviationX = Random.Range(-1f, 1f);
        float deviationZ = Random.Range(-1f, 1f); 
        targetPosition += new Vector3(deviationX, 0, deviationZ);
        Vector3 direction = (targetPosition - spawnPosition).normalized; 
        rb.AddForce(direction * randomForce, ForceMode.Impulse);
    }

    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log("pass");
            playerPass=true;//if pass stop launching rocks and destroy the floor 
            Invoke("StartDestructionSequence",2f);
        }
    }
    public void StartDestructionSequence()
    {
        StartCoroutine(ShakeAndDestroy());
    }
    IEnumerator ShakeAndDestroy()
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        audioSource.clip = shakeSound;
        audioSource.volume = soundVolume;
        audioSource.spatialBlend = 1.0f; 
        audioSource.minDistance = 2.0f;  
        audioSource.loop = true;         
        audioSource.Play();

        while (elapsed < shakeDuration)
        {
            
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
        Destroy(forDestroy);
        
        
    }
}