using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float gametime;
    public Text timerText;

    float startTime;
    float currentTime;
    public bool isTimerRunning;
    public WaitForSeconds oneSecond;

    // Start is called before the first frame update
    void Awake()
    {
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timerText.text = "00:00";

        startTime = Time.time;
        currentTime = Time.time;

        isTimerRunning = true;
        oneSecond = new WaitForSeconds(1f);

        StartCoroutine(UpdateTimer());
    }

    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            if (isTimerRunning)
            {
                gametime = currentTime - startTime;
                timerText.text = FormatTime();
                currentTime++;
            }
            yield return oneSecond;
        }
    }
    public string FormatTime()
    {
        int minutes;
        minutes = Mathf.FloorToInt(gametime/60f);
        gametime = gametime % 60;
        string formatTime = ((int)minutes).ToString("D2") + ":" + ((int)gametime).ToString("D2");
        return formatTime;

    }
}
