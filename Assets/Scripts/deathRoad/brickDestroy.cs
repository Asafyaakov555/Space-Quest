using UnityEngine;

public class brickDestroy : MonoBehaviour
{
    
    private float lifeTimer = 0f;

    public AudioClip hitSound;

    void Update()
    {
        
        lifeTimer += Time.deltaTime;

        
        if (transform.position.y <= -1 || lifeTimer > 3f) //after three seconds the rock destroy
        {
            Destroy(gameObject); 
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        
        float randomPitch = Random.Range(0.8f, 1.2f);
        float volume = collision.relativeVelocity.magnitude / 10f; //rock hit sound

        
        if (sfxSound.Instance != null && hitSound != null)
        {
            sfxSound.Instance.PlaySound(hitSound, volume, randomPitch);
        }
    }
}