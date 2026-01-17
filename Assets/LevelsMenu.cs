using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelsMenu : MonoBehaviour
{
    public void PlayEasy()
    {
        PlayerPrefs.SetInt("Difficulty", 0);
        SceneManager.LoadScene("Gameplay");
    }

    public void PlayMedium()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
        SceneManager.LoadScene("Gameplay");
    }

    public void PlayHard()
    {
        PlayerPrefs.SetInt("Difficulty", 2);
        SceneManager.LoadScene("Gameplay");
    }
}
