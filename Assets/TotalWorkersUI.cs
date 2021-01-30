using UnityEngine;
using UnityEngine.UI;

public class TotalWorkersUI : MonoBehaviour
{
    private string workerTag = "worker";
    void Update()
    {
        var workers = GameObject.FindGameObjectsWithTag(workerTag);
        GetComponent<Text>().text = string.Format("{0} workers", workers.Length);
    }
}
