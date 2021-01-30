using UnityEngine;
using UnityEngine.UI;

public class UpgradeWorkerGatherAmountText : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        var workerGatherAmountUpgradeCost = Shop.instance.GetWorkerGatherAmountCost();
        GetComponent<Text>().text = string.Format("Upgrade gather amount ({0} minerals)", workerGatherAmountUpgradeCost);
    }
}
