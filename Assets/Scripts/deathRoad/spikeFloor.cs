using UnityEngine;

public class spikeFloor : MonoBehaviour
{
    [SerializeField] GameObject[] spikesFirstLine;
    [SerializeField] GameObject[] spikesSecondLine;
    void Start()
    {
        
        ActivateRandomSpikes(spikesFirstLine);
        ActivateRandomSpikes(spikesSecondLine);
    }
    void ActivateRandomSpikes(GameObject[] spikesLine)//  build 2/3 spiks so the player can advance
    {
        
        foreach (var spike in spikesLine)
        {
            spike.SetActive(false);
        }

       
        if (spikesLine.Length < 3) return;

        
        int safePathIndex = Random.Range(0, spikesLine.Length);

        for (int i = 0; i < spikesLine.Length; i++)
        {
            
            if (i != safePathIndex)
            {
                spikesLine[i].SetActive(true);
            }
        }
    }
    




}