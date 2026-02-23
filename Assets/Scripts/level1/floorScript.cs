using System.Collections;
using UnityEngine;

public class floorScript : MonoBehaviour
{
    
    [SerializeField] float shakeDuration = 2.0f;
    [SerializeField] float shakeMagnitude = 0.1f;
    [Header("Audio Settings")]
    [SerializeField] AudioClip shakeSound; 
    [SerializeField] AudioSource audioSource; 
    [SerializeField] [Range(0f, 1f)] float soundVolume = 1f;
    
    
    public void startDestroyProcess(GameObject oldfloor,float delayTime,Level_Genarator level_Genarator,GameObject currentFloor,PlayerFloorTracker tracker)
    {
        
        StartCoroutine(ShakeAndDestroy(oldfloor,delayTime,level_Genarator,currentFloor,tracker));
    }
    
    IEnumerator ShakeAndDestroy(GameObject oldfloor,float delayTime, Level_Genarator generator, GameObject currentFloor, PlayerFloorTracker tracker)
    {
        
        yield return new WaitForSeconds(delayTime);
        Vector3 originalPos = oldfloor.transform.position;
        audioSource.clip = shakeSound;
        audioSource.volume = soundVolume;
        audioSource.spatialBlend = 1.0f; 
        audioSource.minDistance = 2.0f;  
        audioSource.loop = true;         
        audioSource.Play();
        
        float elapsed = 0.0f;
        Transform colorChild = oldfloor.transform.Find("color");
    
   
        if (colorChild != null)
        {
            
            Renderer floorRenderer = colorChild.GetComponent<Renderer>();
            
            if (floorRenderer != null)
            {
                
                floorRenderer.material.color = Color.red;
            }
        }
        
        


        while (elapsed < shakeDuration)
        {
            
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            oldfloor.transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
            
        }
        
        oldfloor.transform.position = originalPos;
        generator.SpawnMaze(currentFloor.transform.position,originalPos);//spawn floors on the current floor of the player
        tracker.removeFrom(oldfloor);// remove from the doomed floors 
        Destroy(oldfloor);//destroy the floor after the shake is over
        
        
        
        
        
    }
}
