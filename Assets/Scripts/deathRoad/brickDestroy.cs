using UnityEngine;

public class brickDestroy : MonoBehaviour
{
    
    private float lifeTimer = 0f;

    public AudioClip hitSound;
    public float rockLife=3f;
    public float minHeight=-1f;
    public float minPitch=0.8f;
    public float maxPitch=1.2f;

    void Update()
    {
        
        lifeTimer += Time.deltaTime;

        
        if (transform.position.y <= minHeight || lifeTimer > rockLife) //after three seconds the rock destroy
        {
            Destroy(gameObject); 
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        
        float randomPitch = Random.Range(minPitch, maxPitch);
        float volume = collision.relativeVelocity.magnitude / 10f; //rock hit sound

        
        if (sfxSound.Instance != null && hitSound != null)
        {
            sfxSound.Instance.PlaySound(hitSound, volume, randomPitch);
        }
    }
}