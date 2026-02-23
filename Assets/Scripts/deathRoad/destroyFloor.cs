using System.Collections;
using UnityEngine;

public class destroyFloor : MonoBehaviour
{
    const string playerstring = "Player";
    [SerializeField] float shakeDuration = 2.0f;
    [SerializeField] GameObject wall;
    [Header("Audio Settings")]
    [SerializeField] AudioClip shakeSound; 
    [SerializeField] AudioSource audioSource; 
    [SerializeField] [Range(0f, 1f)] float soundVolume = 1f;
    
    
    [SerializeField] float shakeMagnitude = 0.1f;
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == playerstring)
        {
            StartDestructionSequence();
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
        Destroy(gameObject);
        if(wall!=null)
        Destroy(wall);
        
        
        
    }
}
