
using UnityEngine;

public class winRoutine : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager_level2 level2=FindAnyObjectByType<GameManager_level2>();
            //level2.Winner();
            GameManager_level2.Instance.Winner();
        }
    }
        
}
