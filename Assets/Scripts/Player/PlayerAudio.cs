using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Components")]
    public AudioSource sfxSource;   // player sfx not the same for all game
    public PlayerMovement playerMovement;

    [Header("SFX Clips")] 
    
    public AudioClip footstepSound;
    public AudioClip jumpSound;      
    public AudioClip landSound;      
    public AudioClip roolSound;  
    public AudioClip hurtSound;
      
    [Header("Settings")]
    [SerializeField] float runningStepsVolume = 0.3f; 
    [SerializeField] float jumpVolume = 0.5f;
    [SerializeField] float pitchRoll = 1.5f;

    
    public void PlayFootstep()
    {
        if (playerMovement != null && playerMovement.isGrounded == false) 
        {
            return; 
        }
        
        PlaySFX(footstepSound, runningStepsVolume);
    }

    public void PlayJumpSound()
    {
        PlaySFX(jumpSound, jumpVolume);
    }

    public void PlayRoolSound()
    {
        PlaySFX(roolSound, jumpVolume, pitchRoll);
    }

    public void PlayhurtSound()
    {
        PlaySFX(hurtSound, jumpVolume);
    }

    

    public void PlaygangamSound()
    {
        if (backGroundMusic.Instance != null)
        {
            backGroundMusic.Instance.PlayGangamStyle();
        }
    }

    public void PlaylalaSound()
    {
        if (backGroundMusic.Instance != null)
        {
            backGroundMusic.Instance.PlayLala();
        }
    }

    public void PlayGodSound()
    {
        if (backGroundMusic.Instance != null)
        {
            backGroundMusic.Instance.PlayGodSound();
        }
    }

    public void StopDanceMusic()
    {
        if (backGroundMusic.Instance != null)
        {
            backGroundMusic.Instance.StopSpecialMusic();
        }
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