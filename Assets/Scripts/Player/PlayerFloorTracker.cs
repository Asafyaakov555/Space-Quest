using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloorTracker : MonoBehaviour
{
    private GameObject currentFloor;
    [SerializeField] Level_Genarator Level_Genarator;

    private float lastTriggerTime = 0f;  
    private float triggerCooldown = 2f;  
    [SerializeField] float shakeDuration = 2.0f;
    
    
    [SerializeField] float shakeMagnitude = 0.1f;
    
    private HashSet<GameObject> doomedFloors = new HashSet<GameObject>();
    

    void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Floor")) return;
        GameObject newFloor = other.gameObject;
        if (doomedFloors.Contains(newFloor)) return;
        if (currentFloor != null && currentFloor != newFloor)// if the player advenace to another floor the prvious floor need to be destroy
        {
            
            if (Time.time - lastTriggerTime < triggerCooldown)
            return;

            lastTriggerTime = Time.time;
            GameObject oldFloor = currentFloor;
            currentFloor = newFloor;
            StartDestructionSequence(oldFloor,currentFloor);
            
        }
        else if (currentFloor == null)
        {
            currentFloor = newFloor;
        }
    }
    public void StartDestructionSequence(GameObject oldfloor,GameObject currentFloor)
    {
        if (doomedFloors.Contains(oldfloor)) return;
        doomedFloors.Add(oldfloor);
        floorScript script = oldfloor.GetComponent<floorScript>();
        script.startDestroyProcess(oldfloor,0.5f,Level_Genarator,currentFloor,this);
        //StartCoroutine(ShakeAndDestroy(oldfloor,0.5f));
    }
    public void removeFrom(GameObject oldFloor)
    {
        doomedFloors.Remove(oldFloor);
        
    }

    
    /*
    IEnumerator ShakeAndDestroy(GameObject oldfloor,float delayTime)
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
        Level_Genarator.SpawnMaze(currentFloor.transform.position,originalPos);
        doomedFloors.Remove(oldfloor);
        Destroy(oldfloor);
        
        
        
    }
    */
    
}
