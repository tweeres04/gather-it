using UnityEngine;
using System.Linq;

static class InitialValues
{
    public static float speed = 5f;
    public static float gatherTime = 3f;
    public static int minGatherAmount = 1;
    public static int maxGatherAmount = 5;
}

public class Worker : MonoBehaviour
{
    private static string workerTag = "worker";
    private static string baseTag = "base";

    private enum State
    {
        Idle,
        MovingToTarget,
        Gathering,
        Delivering,
    }

    private float turnSpeed = 5f;
    private static float speed = 5f;
    private static float speedUpgradeAmount = 2.5f;
    private Color startColor;
    private bool isSelected;
    private bool isHovered;
    private static float gatherTime = 3f;
    private static float gatherTimeUpgradeFactor = 0.25f;
    private float gatherCountdown = 0f;

    private int mineralsHeld = 0;
    private static int minGatherAmount = 1;
    private static int maxGatherAmount = 5;
    private static int gatherUpgradeAmount = 1;
    public GameObject heldMineralPrefab;

    private State state = State.Idle;

    public Mineral target = null;
    private Transform nearestBase;

    public static void resetUpgrades()
    {
        speed = InitialValues.speed;
        gatherTime = InitialValues.gatherTime;
        minGatherAmount = InitialValues.minGatherAmount;
        maxGatherAmount = InitialValues.maxGatherAmount;
    }

    void Awake()
    {
        findNearestUntargetedMineral();
    }

    void Update()
    {

        switch (state)
        {
            case State.MovingToTarget:
                if (target != null)
                {
                    var reachedDestination = MoveTo(target.transform);
                    if (reachedDestination)
                    {
                        var allWorkers = FindObjectsOfType<Worker>();
                        if (!allWorkers.Any(w => w.target == target && w.state == State.Gathering))
                        {
                            state = State.Gathering;
                            gatherCountdown = gatherTime;
                            return;
                        }
                        else
                        {
                            findNearestUntargetedMineral();
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("State.MovingToTarget with no target");
                    state = State.Idle;
                }
                break;

            case State.Gathering:
                gatherCountdown -= Time.deltaTime;
                if (gatherCountdown <= 0f)
                {
                    var mineralsToTake = Random.Range(minGatherAmount, maxGatherAmount);
                    mineralsHeld = target.takeMinerals(mineralsToTake);
                    findNearestBase();
                    instantiateHeldMineral();
                    state = State.Delivering;
                }
                break;

            case State.Delivering:
                {
                    var reachedDestination = MoveTo(nearestBase);
                    if (reachedDestination)
                    {
                        Base.instance.DepositMinerals(mineralsHeld);
                        destroyHeldMineral();
                        state = State.MovingToTarget;
                        return;
                    }
                    break;
                }

            case State.Idle:
                if (target)
                {
                    state = State.MovingToTarget;
                }
                break;
            default:
                throw new System.Exception("Unhandled state!");
        }
    }


    public static bool areAllWorkersIdle()
    {
        var workers = GameObject.FindGameObjectsWithTag(workerTag).Select(w => w.GetComponent<Worker>());
        var totalWorkers = workers.Count();

        var idleWorkersCount = workers.Count(w => w.state == State.Idle);

        return totalWorkers == idleWorkersCount;
    }
    void destroyHeldMineral()
    {
        var heldMineral = transform.GetChild(0);
        GameObject.Destroy(heldMineral.gameObject);
    }

    void instantiateHeldMineral()
    {
        var heldMineral = GameObject.Instantiate(heldMineralPrefab, transform, false);
        heldMineral.transform.localPosition = Vector3.forward * 0.7f;
    }

    void findNearestBase()
    {
        var bases = GameObject.FindGameObjectsWithTag(baseTag);
        float shortestDistance = Mathf.Infinity;
        foreach (var currentBase in bases)
        {
            float distanceToBase = Vector3.Distance(transform.position, currentBase.transform.position);
            if (distanceToBase < shortestDistance)
            {
                shortestDistance = distanceToBase;
                nearestBase = currentBase.transform;
            }
        }
    }

    public void findNearestUntargetedMineral()
    {
        var workers = GameObject.FindGameObjectsWithTag(workerTag);
        var mineralsWithoutTarget = Mineral.findAllMinerals().Where(
            m => m != target &&
            !workers.Select(
                go => go.GetComponent<Worker>()
            ).Any(
                w => w.target == m
            )
        );
        float shortestDistance = Mathf.Infinity;
        Mineral nearestMineral = null;
        foreach (Mineral mineral in mineralsWithoutTarget)
        {
            float distanceToMineral = Vector3.Distance(transform.position, mineral.transform.position);
            if (distanceToMineral < shortestDistance)
            {
                shortestDistance = distanceToMineral;
                nearestMineral = mineral;
            }
        }

        target = nearestMineral;
    }

    bool MoveTo(Transform target, float doneThreshold = 1f)
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude < doneThreshold)
        {
            return true;
        }

        Vector3 moveDir = dir;
        moveDir.y = 0;
        transform.Translate(moveDir.normalized * distanceThisFrame, Space.World);

        return false;
    }

    public static void UpgradeSpeed()
    {
        speed += speedUpgradeAmount;
    }

    public static void UpgradeGatherAmount()
    {
        minGatherAmount += gatherUpgradeAmount;
        maxGatherAmount += gatherUpgradeAmount;
    }

    public static void UpgradeGatherTime()
    {
        gatherTime -= Mathf.Max(0, gatherTimeUpgradeFactor);
    }
}
