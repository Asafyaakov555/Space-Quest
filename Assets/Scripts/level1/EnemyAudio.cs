using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Components")]
    public AudioSource sfxSource;   
    

    [Header("Audio Clips")]
    public AudioClip footstepSound;
    public AudioClip blind;
    
      
    [Header("Settings")]
    [SerializeField] float runningStepsVolume=0.3f; 
    [SerializeField] float blindScreamVolume=0.2f; 
   

    
    public void PlayFootstep()
    {
        
        
        PlaySFX(footstepSound, runningStepsVolume);
    }
     public void Playblindstep()
    {
        
        
        PlaySFX(blind, blindScreamVolume);
    }


    
    
    private void PlaySFX(AudioClip clip, float volume, float? pitch = null)
    {
        if (clip == null || sfxSource == null) return;

        
        
        if (pitch != null)
        {
            sfxSource.pitch = pitch.Value;
        }
        else
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f); 
        }

        sfxSource.PlayOneShot(clip, volume); 
    }
}