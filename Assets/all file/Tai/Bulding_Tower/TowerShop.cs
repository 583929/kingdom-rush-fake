using UnityEngine;

public class TowerShop : MonoBehaviour
{
    public static TowerShop instance;

    [Header("Tower Prefabs")]
    public GameObject iceTowerPrefab;
    public GameObject fireTowerPrefab;
    public GameObject electricTowerPrefab;

    private GameObject selectedTowerPrefab;

    void Awake()
    {
        instance = this;
    }

    public void SelectIceTower()
    {
        selectedTowerPrefab = iceTowerPrefab;
        Debug.Log("Đã chọn trụ băng");
    }

    public void SelectFireTower()
    {
        selectedTowerPrefab = fireTowerPrefab;
        Debug.Log("Đã chọn trụ lửa");
    }

    public void SelectElectricTower()
    {
        selectedTowerPrefab = electricTowerPrefab;
        Debug.Log("Đã chọn trụ điện");
    }

    public GameObject GetSelectedTower()
    {
        return selectedTowerPrefab;
    }
}