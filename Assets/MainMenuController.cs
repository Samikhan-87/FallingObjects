using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenLevels()
    {
        SceneManager.LoadScene("Levels");
    }

    public void mainmenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenAbout()
    {
        SceneManager.LoadScene("About");
    }

    public void OpenRules()
    {
        SceneManager.LoadScene("Rules");
    }

    public void OpenLetsPlay()
    {
        SceneManager.LoadScene("Level Button Scene");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings Page");
    }

}
