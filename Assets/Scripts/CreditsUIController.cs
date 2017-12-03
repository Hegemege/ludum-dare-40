using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CreditsUIController : MonoBehaviour 
{
    void Awake() 
    {
        GameManager.Instance.LevelLoadingDone = true;
    }

    void Start() 
    {
        
    }
    
    void Update() 
    {
        
    }

    public void BackToMenuClick()
    {
        System.GC.Collect();
        SceneManager.LoadScene("MainMenu");
    }
}
