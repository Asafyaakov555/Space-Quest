using UnityEngine;

public class KillerWall : MonoBehaviour
{
    [Header("Movement Settings")]
    
    [SerializeField] float moveDistance = 10f; 

    public float minSpeed = 10f;
    public float maxSpeed = 20f;

    private float speed;
    private bool movingRight;
    private float startX;
    bool hitThePlayer=false;

    void Start()
    {
        
        startX = transform.position.x;
        speed = Random.Range(minSpeed, maxSpeed);
        movingRight = Random.value > 0.5f;
    }

    void Update()
    {
        moveWAll();
    }
    public void changeHitPlayer()
    {
        hitThePlayer=true;
    }
    
    void moveWAll()
    {
        if(hitThePlayer)return;
        float moveStep = speed * Time.deltaTime;

        if (movingRight)
        {
            transform.Translate(Vector3.right * moveStep);// moveing the wall left and right 
            if (transform.position.x >= startX + moveDistance)
            {
                movingRight = false; 
            }
        }
        else
        {
            transform.Translate(Vector3.left * moveStep);

            
            if (transform.position.x <= startX - moveDistance)
            {
                movingRight = true; 
            }
        }
    }
}