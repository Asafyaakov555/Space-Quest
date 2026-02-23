using UnityEngine;

public class trampolineScript : MonoBehaviour
{
    public float jumpForce = 15f;
    
    public AudioClip trampSound;
    [SerializeField] float soundVol=1f;
    

    
    void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            PlayerMovement playerMove = other.gameObject.GetComponent<PlayerMovement>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                Playsound();
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            if(playerMove!=null)
            {
                playerMove.animator.SetBool("useTramp", true);
                playerMove.FunnyAnimation();
                
            }
        }
    }
    

    void Playsound()
    {
        
        if (trampSound==null) return;
        sfxSound.Instance.PlaySound(trampSound,soundVol);
    }

}
