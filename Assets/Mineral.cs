﻿using UnityEngine;
using UnityEngine.EventSystems;

public class Mineral : MonoBehaviour
{
    private string workerTag = "worker";

    public int minerals;

    public void OnMouseDown()
    {
        var workers = GameObject.FindGameObjectsWithTag(workerTag);
        foreach (var worker in workers)
        {
            worker.GetComponent<Worker>().Gather(this);
        }
    }

    public int takeMinerals()
    {
        var mineralsTaken = Mathf.Min(Random.Range(1, 5), minerals);
        minerals -= mineralsTaken;
        if (minerals <= 0)
        {
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