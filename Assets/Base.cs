using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    enum State
    {
        Normal,
        GameOver,
    };

    State state = State.Normal;
    public GameObject gameOver;
    public static Base instance;
    public int minerals = 0;
    public int totalMineralsCollected = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mineral.GetMineralsLeft() == 0 && Worker.areAllWorkersIdle())
        {
            state = State.GameOver;
            gameOver.SetActive(true);
        }
    }

    public void DepositMinerals(int minerals)
    {
        this.minerals += minerals;
        this.totalMineralsCollected += minerals;
    }

    public int getTotalMinerals()
    {
        return minerals;
    }

    public bool isGameOver()
    {
        return state == State.GameOver;
    }
}
