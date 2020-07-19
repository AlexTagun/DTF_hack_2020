using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void LoadSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSceneTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
