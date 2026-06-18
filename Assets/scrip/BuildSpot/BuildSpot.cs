using UnityEngine;

public class BuildSpott : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 50;

    private bool hasTower = false;

    void OnMouseDown()
    {
        if (hasTower)
            return;

        
        if (Money.instance != null && Money.instance.SpendMoney(towerCost))
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