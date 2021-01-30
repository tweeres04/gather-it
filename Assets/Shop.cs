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

    private int workerSpeedCost = 50;
    private int workerGatherAmountCost = 50;

    private float costIncreaseFactor = 1.5f;

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
            workerSpeedCost = (int)Mathf.Round(workerSpeedCost * costIncreaseFactor);
        }
    }

    public void UpgradeWorkerGatherAmount()
    {
        if (Base.instance.minerals >= workerGatherAmountCost)
        {
            Base.instance.minerals -= workerGatherAmountCost;
            Worker.UpgradeGatherAmount();
            workerGatherAmountCost = (int)Mathf.Round(workerGatherAmountCost * costIncreaseFactor);
        }
    }

    public int GetWorkerSpeedCost()
    {
        return workerSpeedCost;
    }

    public int GetWorkerGatherAmountCost()
    {
        return workerGatherAmountCost;
    }
}
