using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToBuilder()
    {
        SceneManager.LoadScene("ArmyBuilder");
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void BackToMain()
    {
        SceneManager.LoadScene("Menu_Main");
    }
}
