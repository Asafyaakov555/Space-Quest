using TMPro;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material[] skyboxOptions; 
    private int currentIndex = 0;
    [SerializeField] TMP_Text ColorTEXT;
    public static SkyboxManager Instance;
    void Awake()
    {
        
        if (Instance != null)
        {
            
            Destroy(gameObject); 
        }
        else
        {
            
            Instance = this;
            
        }
        
    }

    void Start()
    {
        
        currentIndex = PlayerPrefs.GetInt("SelectedSkybox", 0);
        ApplySkyboxChanges(); 
    }

    public void NextSkybox()
    {
        currentIndex++;
        if (currentIndex >= skyboxOptions.Length)
        {
            currentIndex = 0;
        }
        
        SaveAndApply();
    }

    public void PreviousSkybox()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = skyboxOptions.Length - 1;
        }

        SaveAndApply();
    }
    public int getSkyIndex()
    {
        return currentIndex;
    }

    private void SaveAndApply()
    {
        PlayerPrefs.SetInt("SelectedSkybox", currentIndex);
        PlayerPrefs.Save();
        ApplySkyboxChanges();
    }

    
    private void ApplySkyboxChanges()
    {
        if (skyboxOptions.Length == 0) return;

        RenderSettings.skybox = skyboxOptions[currentIndex];
        DynamicGI.UpdateEnvironment(); 
        switch (currentIndex)
        {
            case 0:
                UpdateUIText("Flamingo System", new Color(1f, 0.4f, 0.7f));
                break;
            case 1:
                UpdateUIText("Cosmic Forest", Color.green);
                break;
            case 2:
                UpdateUIText("The Purple Void", new Color(0.75f, 0.4f, 1f));
                break;
        }
    }

    private void UpdateUIText(string text, Color color)
    {
        if (ColorTEXT != null)
        {
            ColorTEXT.text = text;
            ColorTEXT.color = color;
        }
    }
}