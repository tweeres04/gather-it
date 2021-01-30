using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MineralsPerSecondUI : MonoBehaviour
{
    private int lastMinerals = 0;
    private static int historyLength = 10;
    private List<int> mpsHistory = new List<int>(historyLength);

    void Start()
    {
        InvokeRepeating("UpdateText", 0f, 1f);
    }

    void UpdateText()
    {
        var minerals = Base.instance.totalMineralsCollected;
        var mineralsThisSecond = (minerals - lastMinerals);
        mpsHistory.Add(mineralsThisSecond);

        if (mpsHistory.Count >= historyLength)
        {
            mpsHistory.RemoveAt(0);
        }

        var mineralsPerSecond = mpsHistory.Average();
        GetComponent<Text>().text = string.Format("{0:F1} MPS", mineralsPerSecond);

        lastMinerals = minerals;
    }
}
