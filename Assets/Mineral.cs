using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Mineral : MonoBehaviour
{
    private string workerTag = "worker";
    public static string mineralTag = "mineral";

    private int minerals = 15;

    public static IEnumerable<Mineral> findAllMinerals()
    {
        return GameObject.FindGameObjectsWithTag(mineralTag).Select(go => go.GetComponent<Mineral>());
    }

    public static int GetMineralsLeft()
    {
        var mineralsLeft = findAllMinerals()
            .Sum(mineral => mineral.minerals);

        return mineralsLeft;
    }

    public int takeMinerals(int mineralsToTake)
    {
        var mineralsTaken = Mathf.Min(mineralsToTake, minerals);
        minerals -= mineralsTaken;
        if (minerals <= 0)
        {
            var workersTargetingMe = GameObject.FindGameObjectsWithTag(workerTag)
                .Select(go => go.GetComponent<Worker>())
                .Where(w => w.target == this);
            foreach (var worker in workersTargetingMe)
            {
                worker.findNearestUntargetedMineral();
            }
            Destroy(gameObject);
        }
        return mineralsTaken;
    }
}
