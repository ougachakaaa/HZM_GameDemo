using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public TextMeshPro timerTextMP;
    public float startTime;
    public float currentTime;
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
                timerText.text = FormatTime(currentTime - startTime);
                currentTime++;
            }
            yield return oneSecond;
        }
    }
    public string FormatTime(float seconds)
    {
        int minutes;
        minutes = Mathf.FloorToInt(seconds/60f);
        char? zeroM = minutes < 10 ? '0' : null;
        seconds = seconds % 60;
        char? zeroS = seconds < 10 ? '0' : null;
        string formatTime = zeroM + minutes.ToString() + ":" + zeroS + seconds.ToString();
        return formatTime;

    }
}
