using UnityEngine;
using TMPro; // Bắt buộc phải có dòng này để điều khiển TextMeshPro

public class Money : MonoBehaviour
{
    // Tạo một bản bản sao static để các script khác (như Enemy) có thể gọi tới dễ dàng
    public static Money instance;

    [Header("Settings")]
    public int currentMoney = 100; // Số tiền khởi đầu của cháu

    private TextMeshProUGUI moneyText; // Biến để lưu thành phần TextMeshPro

    void Awake()
    {
        // Khởi tạo Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Tự động lấy thành phần TextMeshProUGUI gắn trên chính Object này
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    // Hàm để script quái chết gọi sang nhằm cộng tiền
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI(); // Cộng xong thì cập nhật chữ hiển thị liền
    }

    // Hàm để script xây tháp (BuildSpot) gọi sang nhằm trừ tiền
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    // Hàm cập nhật chữ hiển thị số tiền trên màn hình
    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Tiền: " + currentMoney;
        }
    }
}