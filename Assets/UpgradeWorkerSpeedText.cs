using UnityEngine;
using UnityEngine.UI;

public class UpgradeWorkerSpeedText : MonoBehaviour
{
    void Update()
    {
        var workerSpeedUpgradeCost = Shop.instance.workerSpeedCost;
        GetComponent<Text>().text = string.Format("Upgrade worker speed ({0} minerals)", workerSpeedUpgradeCost);
    }
}
