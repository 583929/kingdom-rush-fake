using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    [Header("Build Position")]
    public Transform buildPoint;

    private bool hasTower = false;
    private GameObject currentTower;

    void OnMouseDown()
    {
        if (hasTower)
        {
            Debug.Log("Ô này đã có trụ rồi");
            return;
        }

        if (BuildMenuUI.instance != null)
        {
            BuildMenuUI.instance.OpenMenu(this, transform.position);
        }
    }

    public void BuildTower(GameObject towerPrefab)
    {
        if (hasTower)
        {
            Debug.Log("Ô này đã có trụ rồi");
            return;
        }

        if (towerPrefab == null)
        {
            Debug.Log("Chưa có prefab trụ");
            return;
        }

        TowerData towerData = towerPrefab.GetComponent<TowerData>();

        if (towerData == null)
        {
            Debug.LogError("Prefab trụ chưa có TowerData!");
            return;
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("Chưa có GameManager!");
            return;
        }

        if (GameManager.instance.SpendMoney(towerData.price) == false)
        {
            Debug.Log("Không đủ tiền để xây trụ");
            return;
        }

        Vector3 spawnPos = transform.position;

        if (buildPoint != null)
        {
            spawnPos = buildPoint.position;
        }

        currentTower = Instantiate(
            towerPrefab,
            spawnPos,
            Quaternion.identity
        );

        hasTower = true;

        if (BuildMenuUI.instance != null)
        {
            BuildMenuUI.instance.CloseMenu();
        }

        Debug.Log("Đã xây trụ: " + towerPrefab.name);
    }
}