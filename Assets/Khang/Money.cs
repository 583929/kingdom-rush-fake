using UnityEngine;
<<<<<<< Updated upstream
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
=======
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
>>>>>>> Stashed changes
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
<<<<<<< Updated upstream
        }

        // Tự động lấy thành phần TextMeshProUGUI gắn trên chính Object này
        moneyText = GetComponent<TextMeshProUGUI>();
=======
            return;
        }

        if (moneyText == null)
        {
            moneyText = GetComponent<TextMeshProUGUI>();
        }
>>>>>>> Stashed changes
    }

    void Start()
    {
        UpdateMoneyUI();
    }

<<<<<<< Updated upstream
    // Hàm để script quái chết gọi sang nhằm cộng tiền
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI(); // Cộng xong thì cập nhật chữ hiển thị liền
    }

    // Hàm để script xây tháp (BuildSpot) gọi sang nhằm trừ tiền
=======
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    // Hàm cập nhật chữ hiển thị số tiền trên màn hình
=======
>>>>>>> Stashed changes
    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Tiền: " + currentMoney;
        }
    }
}