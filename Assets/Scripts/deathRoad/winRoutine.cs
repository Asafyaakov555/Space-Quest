
using UnityEngine;

public class winRoutine : MonoBehaviour
{
    const string playerstring = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerstring))
        {
            //GameManager_level2 level2=FindAnyObjectByType<GameManager_level2>();
            //level2.Winner();
            GameManager_level2.Instance.Winner();
        }
    }
        
}
