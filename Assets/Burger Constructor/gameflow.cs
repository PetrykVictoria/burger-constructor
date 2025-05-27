using UnityEngine;
using TMPro;

public class gameflow : MonoBehaviour
{
    // Тепер — лише по одній тарілці й одному замовленню
    public int orderValue = 0;
    public int plateValue = 0;
    public float orderTimer = 60f;

    [Header("UI Elements (TextMeshPro)")]
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI plateText;
    public TextMeshProUGUI resultText;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Оновлюємо таймер
        orderTimer -= Time.deltaTime;

        // Оновлюємо UI що кадр
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (orderText != null)
            orderText.text = $"Order: {orderValue}";

        if (plateText != null)
            plateText.text = $"Plate: {plateValue}";
    }
    public void SetOrderValue(int newValue)
    {
        orderValue = newValue;
        // одразу оновити UI
        UpdateUI();
    }


    public void CheckOrder()
    {
        if (resultText == null) return;

        if (plateValue == orderValue)
        {
            resultText.text = "Correct!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Wrong!";
            resultText.color = Color.red;
        }
    }
}
