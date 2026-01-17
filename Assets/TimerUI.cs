using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    Text timerText;

    void Start()
    {
        timerText = GetComponent<Text>();

        if (GameManager.instance.currentDifficulty == GameDifficulty.Hard)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!GameManager.instance.useTimer) return;

        timerText.text = Mathf.Ceil(
            GameManager.instance.timeRemaining
        ).ToString();
    }
}
