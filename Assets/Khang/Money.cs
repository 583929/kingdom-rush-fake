using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public static Money instance;

    [Header("Settings")]
    public int currentMoney = 100;

    [Header("UI Reference")]
    public TextMeshProUGUI moneyText;

    void Awake()
    {
        // Khởi tạo Singleton để các script khác dễ dàng truy cập bằng Money.instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Nếu quên chưa kéo thả Text vào Inspector, tự động tìm trên chính Object này
        if (moneyText == null)
        {
            moneyText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    // Hàm để script quái gọi khi chết nhằm cộng tiền thưởng
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    // Hàm để script xây tháp gọi sang nhằm kiểm tra và trừ tiền
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true; // Trả về true nếu đủ tiền và trừ tiền thành công
        }
        return false; // Trả về false nếu không đủ tiền
    }

    // Hàm cập nhật chữ hiển thị số tiền trên màn hình UI
    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Tiền: " + currentMoney;
        }
    }
}