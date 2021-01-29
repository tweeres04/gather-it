using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static Base instance;
    public int minerals = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DepositMinerals(int minerals)
    {
        this.minerals += minerals;
        print("Total minerals: " + this.minerals);
    }

    public int getTotalMinerals()
    {
        return minerals;
    }
}
