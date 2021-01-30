using UnityEngine;
using System.Linq;

public class Worker : MonoBehaviour
{
    private string workerTag = "worker";
    private string mineralTag = "mineral";

    private enum State
    {
        Idle,
        MovingToTarget,
        Gathering,
        Delivering,
    }

    private float turnSpeed = 5;
    private float speed = 5;
    private Color startColor;
    private bool isSelected;
    private bool isHovered;
    private float gatherTime = 3f;
    private float gatherCountdown = 0f;

    private int mineralsHeld = 0;

    private State state = State.Idle;

    public Mineral target = null;

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
                    throw new System.Exception("State.MovingToTarget with no target");
                }
                break;

            case State.Gathering:
                gatherCountdown -= Time.deltaTime;
                if (gatherCountdown <= 0f)
                {
                    mineralsHeld = target.takeMinerals();
                    state = State.Delivering;
                }
                break;

            case State.Delivering:
                {
                    var reachedDestination = MoveTo(Base.instance.transform);
                    if (reachedDestination)
                    {
                        Base.instance.DepositMinerals(mineralsHeld);
                        state = State.MovingToTarget;
                        return;
                    }
                    break;
                }

            case State.Idle:
            default:
                // Don't do anything
                break;
        }
    }

    public void findNearestUntargetedMineral()
    {
        var minerals = GameObject.FindGameObjectsWithTag(mineralTag);
        var workers = GameObject.FindGameObjectsWithTag(workerTag);
        var mineralsWithoutTarget = minerals.Select(
            go => go.GetComponent<Mineral>()
        ).Where(
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

    public void SetTarget(Mineral mineral)
    {
        target = mineral;
        if (state != State.Delivering)
        {
            state = State.MovingToTarget;
        }
    }
}
