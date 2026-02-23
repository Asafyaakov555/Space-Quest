using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.Video;

public class levelGenDEATHROAD : MonoBehaviour
{
    [SerializeField] GameObject[] floors;
    int levelLength = 7; 
    private float floorSize = 10f;
    [SerializeField] GameObject roadParent;
    
   
    void Start()
    {
        GenerateLevel();
        
    }
    void GenerateLevel()
    {
        
        Vector3 spawnPosition = roadParent.transform.position;
        spawnPosition.z+=10f;
        
        int randomIndex=0;
        int previousIndex=-1;

        for (int i = 0; i < levelLength; i++)
        {
            GameObject prefabToSpawn;
            if (i == levelLength - 1)
            {
                Quaternion rotation = Quaternion.Euler(0, 90, 0);
                prefabToSpawn = floors[0];
                Instantiate(prefabToSpawn, spawnPosition, rotation,roadParent.transform);
                break;
            }
            else
            {
                if (i > 0)
                previousIndex = randomIndex;
                if(previousIndex>=4)
                randomIndex = Random.Range(1, 4);
                if(previousIndex==1)
                {
                    
                    if (Random.value < 0.5f) 
                    {
                        randomIndex = 1;
                    }
                    else
                    {
                        randomIndex = Random.Range(3, floors.Length);
                        
                    }
                    //we dont after boulder moving walls because it will block the rocks 
                }
                if((previousIndex>1 && previousIndex<4)|| previousIndex==-1)//the second term for the first iteration
                randomIndex = Random.Range(1, floors.Length);
                prefabToSpawn = floors[randomIndex];
                
            }
            if(randomIndex>=4)
            {
                spawnPosition.z -= 3f;
                
            }

            
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity,roadParent.transform);
            // adujst the disatnce between each type(of floor )
            if(randomIndex==1)
            {
                spawnPosition.z += 20f;
                continue;
            }
            if(randomIndex>=4)
            {
                spawnPosition.z += 30f;
                continue;
            }

            
            spawnPosition.z += floorSize;
        }
    }
    
    
    
   
    


    
    
    

}
