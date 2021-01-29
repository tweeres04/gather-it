using UnityEngine;
using System.Linq;

public class Worker : MonoBehaviour
{
    static int instanceCount = 0;

    private int id;

    private enum State
    {
        Idle,
        MovingToTarget,
        Gathering,
        Delivering,
    }

    public Color hoverColor = new Color(89, 146, 192);

    private float turnSpeed = 5;
    private float speed = 5;
    private Renderer rend;
    private Color startColor;
    private bool isSelected;
    private bool isHovered;
    private float gatherTime = 3f;
    private float gatherCountdown = 0f;

    private int mineralsHeld = 0;

    private State state = State.Idle;

    private Mineral target = null;

    // Start is called before the first frame update
    void Start()
    {
        id = ++instanceCount;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void Update()
    {
        if (isSelected || isHovered)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = startColor;
        }

        if (state == State.MovingToTarget)
        {
            if (target != null)
            {
                var reachedDestination = MoveTo(target.transform);
                if (reachedDestination)
                {
                    var allWorkers = FindObjectsOfType<Worker>();
                    if (!allWorkers.Any(w => w.target == target && w.state == State.Gathering))
                    {
                        print("starting to gather");
                        state = State.Gathering;
                        gatherCountdown = gatherTime;
                        return;
                    }
                    else
                    {
                        print("someone else gathering this mineral");
                        findNearestUntargetedMineral();
                    }
                }
            }
            else
            {
                throw new System.Exception("State.MovingToTarget with no target");
            }
        }
        else if (state == State.Gathering)
        {
            gatherCountdown -= Time.deltaTime;
            if (gatherCountdown <= 0f)
            {
                mineralsHeld = target.takeMinerals();
                state = State.Delivering;
            }
        }
        else if (state == State.Delivering)
        {
            var reachedDestination = MoveTo(Base.instance.transform);
            if (reachedDestination)
            {
                Base.instance.DepositMinerals(mineralsHeld);
                state = State.MovingToTarget;
                return;
            }
        }
        else if (state == State.Idle)
        {
            // Don't do anything
        }
    }

    void findNearestUntargetedMineral()
    {
        print("finding nearest mineral");
        Mineral[] minerals = FindObjectsOfType<Mineral>();
        Worker[] workers = FindObjectsOfType<Worker>();
        var mineralsWithoutTarget = minerals.Where(m => m != target && !workers.Any(w => w.target == m));
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

    public void Gather(Mineral mineral)
    {
        target = mineral;
        state = State.MovingToTarget;
    }
}
