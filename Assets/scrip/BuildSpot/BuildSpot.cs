using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 50;

    private bool hasTower = false;

    void OnMouseDown()
    {
        if (hasTower)
            return;

        if (GameManager.instance != null && GameManager.instance.SpendMoney(towerCost))
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            hasTower = true;
        }
        else
        {
            Debug.Log("Không đủ tiền để xây tower");
        }
    }
}