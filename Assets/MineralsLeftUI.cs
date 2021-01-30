using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MineralsLeftUI : MonoBehaviour
{
    void Update()
    {
        var minerals = Mineral
            .findAllMinerals()
            .Sum(mineral => mineral.minerals);

        GetComponent<Text>().text = string.Format("{0} minerals left", minerals);
    }
}
