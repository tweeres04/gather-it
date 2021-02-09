using UnityEngine;

public class Ground : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Shop.instance.IsInBaseBuildingMode())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Shop.instance.BuildBase(hit.point);
            }
        }
    }
}
