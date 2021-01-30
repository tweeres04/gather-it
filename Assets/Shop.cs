using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject workerPrefab;
    private Transform[] spawnPoints = new Transform[4];
    private Transform spawnPoint;
    private int spawnPointIndex = 0;

    private int workerCost = 10;

    void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoint").Select(go => go.transform).ToArray();
        spawnPoint = spawnPoints[0];
    }
    public void BuildWorker()
    {
        if (Base.instance.minerals >= workerCost)
        {
            Base.instance.minerals -= workerCost;
            Instantiate(workerPrefab, spawnPoint.position, Quaternion.identity);
            spawnPoint = spawnPoints[spawnPointIndex % 4];
            spawnPointIndex += 1;
        }
    }
}
