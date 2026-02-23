using UnityEngine;

public class spiksForMovingWall : MonoBehaviour
{
    [SerializeField] GameObject wall;
    const string playerstring = "Player";
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == playerstring)
        {
           KillerWall killer= wall.GetComponent<KillerWall>();
           if(killer!=null)
            {
                killer.changeHitPlayer();
            }
        }
    }

}
