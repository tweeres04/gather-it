﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Mineral : MonoBehaviour
{
    private string workerTag = "worker";
    public static string mineralTag = "mineral";

    public int minerals;

    public static IEnumerable<Mineral> findAllMinerals()
    {
        return GameObject.FindGameObjectsWithTag(mineralTag).Select(go => go.GetComponent<Mineral>());
    }

    public void OnMouseDown()
    {
        var workers = GameObject.FindGameObjectsWithTag(workerTag);
        foreach (var worker in workers)
        {
            worker.GetComponent<Worker>().SetTarget(this);
        }
    }

    public int takeMinerals()
    {
        var mineralsTaken = Mathf.Min(Random.Range(1, 5), minerals);
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

    // Start is called before the first frame update
    void Start()
    {
        minerals = Random.Range(10, 20);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
