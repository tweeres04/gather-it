using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text timeTakenText;

    void OnEnable()
    {
        var time = Time.timeSinceLevelLoad;

        var minutes = Mathf.Floor(time / 60); //Divide the time by sixty to get the minutes.
        var seconds = Mathf.Floor(time % 60);//Use the euclidean division for the seconds.
        var fraction = Mathf.Floor((time * 10) % 10);

        var textTime = String.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, fraction);

        timeTakenText.text = textTime;
    }

    public static void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
