using UnityEngine;
using System.Linq;

public class Mineral : MonoBehaviour
{
    private string workerTag = "worker";

    public int minerals;

    public void OnMouseDown()
    {
        var workers = GameObject.FindGameObjectsWithTag(workerTag);
        foreach (var worker in workers)
        {
            worker.GetComponent<Worker>().SetTarget(this);
        }
    }

    public int takeMinerals()
    {
        var mineralsTaken = Mathf.Min(Random.Range(1, 5), minerals);
        minerals -= mineralsTaken;
        if (minerals <= 0)
        {
            var workersTargetingMe = GameObject.FindGameObjectsWithTag(workerTag)
                .Select(go => go.GetComponent<Worker>())
                .Where(w => w.target == this);
            foreach (var worker in workersTargetingMe)
            {
                worker.findNearestUntargetedMineral();
            }
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
