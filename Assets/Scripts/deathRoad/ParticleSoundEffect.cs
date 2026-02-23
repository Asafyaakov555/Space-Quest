using UnityEngine;

public class ParticleSoundEffect : MonoBehaviour
{
    [Header("Dependencies")]
    public ParticleSystem fireworkParticles;
    public AudioSource audioSource;
    public AudioClip explosionClip;

    [Header("Settings")]
    
    public float minPitch = 0.8f; 
    public float maxPitch = 1.2f;
    public float volume=0.5f;

    private int _lastParticleCount = 0;

    void Start()
    {
        
        if (fireworkParticles == null) fireworkParticles = GetComponent<ParticleSystem>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()// to identify when the fireworks explodes
    {
        
        int currentParticleCount = fireworkParticles.particleCount;
        if (currentParticleCount > _lastParticleCount)
        {
            PlayExplosionSound();
        }
        _lastParticleCount = currentParticleCount;
    }

    void PlayExplosionSound()
    {
        
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume=volume;
        audioSource.PlayOneShot(explosionClip);
    }
}