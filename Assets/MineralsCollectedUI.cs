using UnityEngine;
using UnityEngine.UI;

public class MineralsCollectedUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = Base.instance.minerals.ToString();
    }
}
