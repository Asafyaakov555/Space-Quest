using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [Header("Enemy Settings")]
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;
    private Rigidbody rb;
    bool EnemyVictory = false;
    bool GodMode=false;
    private NavMeshAgent agent;
    BoxCollider attackCo;
    private float nextAttackTime = 0f;
    bool attacking=false;
    bool hit=false;
    [SerializeField] float monsterFallYThreshold = -1f;
    public void EnemyVICTORY()
    {
        EnemyVictory = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       attackCo=GetComponent<BoxCollider>();

        if (player == null) 
        {
            player = GameObject.FindWithTag("Player");
        }
        agent = GetComponent<NavMeshAgent>();
        agent.enabled=true;
    }
    void Update()
    {
        
        if (!EnemyVictory || GodMode)
            followPlayer();
        else
            EnemyWIN(transform.position);

        
        createAttack();
        if(GodMode)//only if godmode is activated the enemy can be blined 
        turnOnBlindAnimation();
        CheckGround();
        monsterFall();
    }
    void monsterFall()
    {
        if (transform.position.y <= monsterFallYThreshold)
        {
            GameManager.Instance.MonsterFall();
        }
    }
    void CheckGround()
    {
        
        bool grounded = Physics.Raycast(transform.position + Vector3.up * 1.0f, Vector3.down, 1.5f);
        if(rb==null)
        {
            Debug.LogWarning("Missing Rigidbody for enemy");
            return;
        }
        // to know if the enemy  on the ground we check with raycast if the enemy dont on the ground 
        //we need to enable him to fall .
        if (!grounded)
        {
            
            rb.useGravity = true;
            rb.isKinematic=false;
            agent.enabled = false; 
        }
        if(grounded && !GodMode && !attacking)
        {
            rb.useGravity = false;
            rb.isKinematic=true;
            agent.enabled = true;; 
        }
        
    }
    private float nextblindANIMTime = 0f;

    void turnOnBlindAnimation()
    {
        if (Time.time < nextblindANIMTime)
            return;
        if(player==null)
        return;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if(distance < 3f )
        {
            
            animator.SetTrigger("blind");
            agent.enabled = false;
            nextblindANIMTime = Time.time + 3f;
            Invoke("resumeAgent",2f);
        }
          
        
    }
    public void GodModeEnabled()
    {
        GodMode=true;
        Invoke("finishGodMode",10f);

    }
    
    void finishGodMode()
    {
        GodMode=false;
        animator.SetTrigger("run");
    }
    
    void createAttack()
    { 
        
        if(GodMode||hit)
            return;
        if(player==null)
        return;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (Time.time < nextAttackTime)
            return;

        if (distance < 0.8f)
        {
            attacking=true;
            nextAttackTime = Time.time + 0.3f;
            agent.enabled = false; 
            animator.SetTrigger("attack");
            //Invoke("attackCollider",0.4f);
            Invoke("resumeAgent",1f);

        }
    }
    public void attackCollider()
    {

        attackCo.enabled=true;
    }
    void resumeAgent()
    {
        agent.enabled = true;
        attackCo.enabled=false;
        attacking=false;
    }
    void EnemyWIN(Vector3 vec)
    {
        transform.position = vec;
        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Player")
        {
            //PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.loser(true);
                EnemyWIN(transform.position);
                animator.enabled = false;
                hit = true;
            }
            
        }
    }
    
    
    void followPlayer()
    {
        
        if (agent.isOnNavMesh)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0 && !clipInfo[0].clip.name.Contains("Running"))
            {
                animator.SetTrigger("run");
            }

            agent.SetDestination(player.transform.position);
            
        }
    }
    public void turnOffANIMATE()
    {
        animator.enabled = false;
    }

}