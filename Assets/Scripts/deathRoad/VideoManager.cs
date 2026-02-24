using UnityEngine;
using UnityEngine.Video; 

public class VideoManger : MonoBehaviour
{
    [Header("Settings")]
    public VideoPlayer videoPlayer; 
    
    [Header("All My Videos")]
    public VideoClip[] movieLibrary; 

    public void PlayVideo(int index)
    {
        if (index >= 0 && index < movieLibrary.Length)
        {
            videoPlayer.clip = movieLibrary[index];
            videoPlayer.Play();
        }
        
    }

   
    public void StopVideo()
    {
        videoPlayer.Stop();
    }
}