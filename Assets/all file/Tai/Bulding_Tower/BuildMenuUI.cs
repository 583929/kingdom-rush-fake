using UnityEngine;

public class BuildMenuUI : MonoBehaviour
{
    public static BuildMenuUI instance;

    [Header("Tower Prefabs")]
    public GameObject iceTowerPrefab;
    public GameObject fireTowerPrefab;
    public GameObject electricTowerPrefab;

    [Header("UI")]
    public GameObject menuPanel;
    public Camera mainCamera;

    [Header("Menu Position")]
    public Vector2 menuOffset = new Vector2(0f, 120f);

    private BuildSpot currentBuildSpot;

    void Awake()
    {
        instance = this;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        CloseMenu();
    }

    public void OpenMenu(BuildSpot buildSpot, Vector3 worldPosition)
    {
        currentBuildSpot = buildSpot;

        if (menuPanel == null)
        {
            Debug.LogError("Chưa kéo BuildMenuPanel vào BuildMenuUI");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Chưa có Main Camera");
            return;
        }

        menuPanel.SetActive(true);

        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);

        screenPos.x += menuOffset.x;
        screenPos.y += menuOffset.y;

        menuPanel.transform.position = screenPos;
    }

    public void CloseMenu()
    {
        currentBuildSpot = null;

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    public void BuyIceTower()
    {
        if (currentBuildSpot != null)
        {
            currentBuildSpot.BuildTower(iceTowerPrefab);
        }
    }

    public void BuyFireTower()
    {
        if (currentBuildSpot != null)
        {
            currentBuildSpot.BuildTower(fireTowerPrefab);
        }
    }

    public void BuyElectricTower()
    {
        if (currentBuildSpot != null)
        {
            currentBuildSpot.BuildTower(electricTowerPrefab);
        }
    }
}