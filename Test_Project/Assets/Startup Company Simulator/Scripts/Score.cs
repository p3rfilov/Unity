using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text score;
    public Text time;
    public Text deaths;

    static float timer;

    void Update()
    {
        timer += Time.deltaTime;
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);
        string minutes_str = minutes.ToString();
        string seconds_str = seconds.ToString();

        if (minutes < 10)
        {
            minutes_str = "0" + minutes.ToString();
        }
        if (seconds < 10)
        {
            seconds_str = "0" + Mathf.RoundToInt(seconds).ToString();
        }

        time.text = minutes_str + ":" + seconds_str;
    }

    public void IncrementScore()
    {
        score.text = (int.Parse(score.text) + 1).ToString();
    }

    public void IncrementDeaths()
    {
        deaths.text = (int.Parse(deaths.text) + 1).ToString();
    }
}
