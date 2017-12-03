using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour 
{
    void Awake() 
    {
        
    }

    void Start() 
    {
        
    }
    
    void Update() 
    {
        
    }

    public void PlayButtonClicked()
    {
        System.GC.Collect();
        SceneManager.LoadSceneAsync("Level1");
    }
}
