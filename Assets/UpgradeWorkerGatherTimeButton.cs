using UnityEngine;
using UnityEngine.UI;

public class UpgradeWorkerGatherTimeButton : MonoBehaviour
{
    void Update()
    {
        var workerGatherTimeUpgradeCost = Shop.instance.GetWorkerGatherTimeCost();
        GetComponent<Text>().text = string.Format("Upgrade gather time ({0} minerals)", workerGatherTimeUpgradeCost);
    }
}
