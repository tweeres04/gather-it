using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeElapsedUI : MonoBehaviour
{
    void Update()
    {
        if (!Base.instance.isGameOver())
        {
            var time = Time.timeSinceLevelLoad;

            var minutes = Mathf.Floor(time / 60); //Divide the time by sixty to get the minutes.
            var seconds = Mathf.Floor(time % 60);//Use the euclidean division for the seconds.
            var fraction = Mathf.Floor((time * 10) % 10);

            var textTime = String.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, fraction);

            GetComponent<Text>().text = textTime;
        }
    }
}
