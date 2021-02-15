using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    enum State
    {
        Normal,
        BaseBuilding,
    }

    private State state = State.Normal;
    public GameObject basePrefab;
    public Transform basesGroup;
    private int baseCost = 50;
    private GameObject baseGhost;

    public GameObject workerPrefab;
    public Transform workersGroup;
    private Transform[] spawnPoints = new Transform[4];
    private Transform spawnPoint;
    private int spawnPointIndex = 0;
    private int workerCost = 10;
    private int workerSpeedCost = 25;
    private int workerGatherAmountCost = 25;
    private int workerGatherTimeCost = 25;

    private float costIncreaseFactor = 1.5f;

    public static Shop instance;

    void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoint").Select(go => go.transform).ToArray();
        spawnPoint = spawnPoints[0];
        instance = this;
    }

    public void EnterBaseBuildingMode()
    {
        state = State.BaseBuilding;
        baseGhost = GameObject.Instantiate(basePrefab, basesGroup);
        baseGhost.tag = "Untagged";
        var renderer = baseGhost.GetComponent<Renderer>();
        var newColor = renderer.material.color;
        newColor.a = 0.5f;
        renderer.material.color = newColor;
    }

    public void ExitBaseBuildingMode()
    {
        GameObject.Destroy(baseGhost);
        state = State.Normal;
    }

    public void BuildBase(Vector3 point)
    {
        if (Base.instance.minerals >= baseCost)
        {
            Base.instance.minerals -= baseCost;
            Instantiate(basePrefab, point, Quaternion.identity, basesGroup);
            ExitBaseBuildingMode();
        }
    }

    void Update()
    {
        if (state == State.BaseBuilding)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                baseGhost.transform.position = hit.point;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                ExitBaseBuildingMode();
            }
        }
    }

    public bool IsInBaseBuildingMode()
    {
        return state == State.BaseBuilding;
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

    public void UpgradeWorkerGatherTime()
    {
        if (Base.instance.minerals >= workerGatherTimeCost)
        {
            Base.instance.minerals -= workerGatherTimeCost;
            Worker.UpgradeGatherTime();
            workerGatherTimeCost = (int)Mathf.Round(workerGatherTimeCost * costIncreaseFactor);
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

    public int GetWorkerGatherTimeCost()
    {
        return workerGatherTimeCost;
    }
}
