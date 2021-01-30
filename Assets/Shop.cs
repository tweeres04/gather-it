using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject workerPrefab;
    public Transform workersGroup;
    private Transform[] spawnPoints = new Transform[4];
    private Transform spawnPoint;
    private int spawnPointIndex = 0;

    private int workerCost = 10;

    public int workerSpeedCost = 100;

    public static Shop instance;

    void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoint").Select(go => go.transform).ToArray();
        spawnPoint = spawnPoints[0];
        instance = this;
    }
    public void BuildWorker()
    {
        if (Base.instance.minerals >= workerCost)
        {
            Base.instance.minerals -= workerCost;
            var newWorker = Instantiate(workerPrefab, spawnPoint.position, Quaternion.identity, workersGroup);
            newWorker.GetComponent<Worker>().findNearestUntargetedMineral();
            spawnPoint = spawnPoints[spawnPointIndex % 4];
            spawnPointIndex += 1;
        }
    }

    public void UpgradeWorkerSpeed()
    {
        if (Base.instance.minerals >= workerSpeedCost)
        {
            Base.instance.minerals -= workerSpeedCost;
            Worker.UpgradeSpeed();
            workerSpeedCost = (int)Mathf.Round(workerSpeedCost * 1.5f);
        }
    }
}
