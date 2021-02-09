using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MineralsLeftUI : MonoBehaviour
{
    void Update()
    {
        var mineralsLeft = Mineral.GetMineralsLeft();

        GetComponent<Text>().text = string.Format("{0} minerals left", mineralsLeft);
    }
}
