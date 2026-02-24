using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("level1")]
    [SerializeField] Level_Genarator level_Genarator;
    public GameManager gameManager;
    public GameManager_level2 gameManager_Level2;
    bool godModeEnable=false;
    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] float rotationSpeed = 2f;
    public float fallMultiplier = 8f;
    [Header("Scripts and objects")]

    [SerializeField] PlayerAudio playerAudio;
    public Animator animator;
    Rigidbody rb;
    CapsuleCollider col; 
    
    public float standingHeight = 2.04f;
    public Vector3 standingCenter = new Vector3(0, 0.92f, 0);
    public float rollingHeight = 1.0f;
    public Vector3 rollingCenter = new Vector3(0, 0.5f, 0);
    Vector3 direction = Vector3.zero;
    
    
    bool live = true;

    public bool jumpRequest = false;
    public bool rollRequest = false;
    public  float jumpForce = 8f;
    
    [Header("deathRoads")]
    
    
    bool deathRoad = false;
    bool winner=false;
    [Header("Ground Check")]
    public Transform groundCheck;      
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public bool isGrounded = true;
    public float godModeTime=10f;
    
    

   
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName=="Level_1")
        {
            deathRoad = false;
            //Debug.Log("deathroad=false;");
        }
        if(sceneName=="deathRoad")
        {
            deathRoad = true;
           // Debug.Log("deathroad=true;");
        }
    }
    
   
    void Update()
    {
        if(winner)return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequest = true;
            
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Debug.Log("isGROUNDED:"+isGrounded);
        if(isGrounded)
        {
            animator.SetBool("useTramp", false);
            playerAudio.StopDanceMusic();
        }
        

        
        if (Input.GetKeyDown(KeyCode.R) && isGrounded)
        {
            
            rollRequest=true;
        }
        if(transform.position.y<-1)
        {
            live=false;
            if(deathRoad)
            {
                gameManager_Level2.lose();
                Destroy(gameObject);
                return;
            }
            gameManager.lose();
            Destroy(gameObject);
        }
        

        
        
    }
    public void FunnyAnimation()
    {
        int randomIndex = Random.Range(0, 2);
        if(randomIndex==0)
        {
            animator.SetTrigger("gangam");
        }
        else
        {
            animator.SetTrigger("lala");
            
        }
            
        
    }
    /*
    void OnDrawGizmosSelected() 
    {
        
        if (groundCheck != null)
        {
            
            Gizmos.color = Color.red;
            
            
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
    */
    

    
    void FixedUpdate()
    {
        
        if (live)
        {
            
            if (rb.linearVelocity.y < -0.1)//to make jump more realistic
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            } 
            if (jumpRequest)
            {
                HandleJump(); 
            } 
            playerMovement();
            if (rollRequest) HandleRoll();
            
        }
    }
    void playerMovement()
    {
        
        if(winner)return;
        var verticalInput = Input.GetAxis("Vertical");   
        var horizontalInput = Input.GetAxis("Horizontal");
        if(!deathRoad)//death road is differant camare settings
        {
            
            Transform camTransform = Camera.main.transform;
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            direction = (camForward * verticalInput) + (camRight * horizontalInput);   
        }
        else
        {
            direction = new Vector3(horizontalInput, 0, verticalInput);
        }
        transform.position += direction * speed * Time.fixedDeltaTime;
        bool IsMoving = direction.magnitude > 0.1f;
        animator.SetBool("IsMoving", IsMoving);
        
        
        if (IsMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        
        
        if (collision.collider.CompareTag("spikes"))
        {
            if(deathRoad)
            {
                animator.SetTrigger("death");
                rb.isKinematic = true; 
                Invoke("freezePlayer",2f);
                gameManager_Level2.lose();
                return;
            }
            
            loser(true);
        }
    }
    void freezePlayer()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Time.timeScale = 0f;
        
    }
    
    public void GodMode()
    {
        playerAudio.PlayGodSound();
        rb.isKinematic=true;
        godModeEnable=true;
        Invoke("disableGodMode",godModeTime);//god mode is 10 seconds
        
    }
    void disableGodMode()
    {
        rb.isKinematic=false;
        godModeEnable=false;
    }
    void HandleJump()
    {
        
        CancelInvoke("changeColliderPOS"); 
        animator.SetTrigger("jump");
        Invoke("returnGround",1f);
        rb.isKinematic=false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpRequest = false;
        
        
        
    }
    void returnGround()
    {
        
        if(godModeEnable)// to enable the player to jump in god mode
        Invoke("returnKinmatic",0.1f);
        
    }
    void returnKinmatic()
    {
        transform.position=new Vector3(transform.position.x,0.03f,transform.position.z);
        rb.isKinematic=true;
    }
    void HandleRoll()
    {
        
        animator.applyRootMotion=true;
        col.height = rollingHeight;
        col.center = rollingCenter;
        animator.SetTrigger("roll");
        Invoke("changeColliderPOS",1f);
        rollRequest = false;
        //rb.linearVelocity = Vector3.zero;
        
        
    }
    void changeColliderPOS()
    {
        col.height = standingHeight;
        col.center = standingCenter;
        animator.applyRootMotion=false;//
    }
    

    public void win()
    {
        winner=true;
        animator.SetTrigger("victory");
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic=true;

    }
    public void loser(bool deathByAttack)
    {
        StopAllCoroutines();
        live=false;
         playerAudio.StopDanceMusic();
        if(deathByAttack)
        {
            if(deathRoad)return;
            animator.SetTrigger("death");
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameManager.lose();
            return;
        }
        


        
    }
    
    public static bool IsAnimationFinised(Animator animator, string AnimationName)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        
        if (!info.IsName(AnimationName)) return true;
        
        if (info.normalizedTime >= 1)
            return true;
        
        return false;
    }
}
    
