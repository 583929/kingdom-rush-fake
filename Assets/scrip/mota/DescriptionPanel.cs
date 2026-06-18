using UnityEngine;

public class DescriptionPanel : MonoBehaviour
{
    public GameObject descriptionPanel;

    void Start()
    {
        // Hiện bảng hướng dẫn khi mới vào game
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(true);
        }

        // Dừng game lại
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        // Tắt bảng hướng dẫn
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }

        // Cho game chạy lại
        Time.timeScale = 1f;

        // Bắt đầu wave sau khi bấm nút
        if (WaveManager.instance != null)
        {
            WaveManager.instance.BeginWaves();
        }
    }
}