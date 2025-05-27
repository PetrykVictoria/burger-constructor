using UnityEngine;
using TMPro;

public class OrderGenerator : MonoBehaviour
{
    [Header("UI Bindings")]
    [Tooltip("Контейнер (Content) під Vertical Layout")]
    public Transform contentPanel;
    [Tooltip("Префаб одного рядка меню (TextMeshPro)")]
    public TextMeshProUGUI orderLinePrefab;

    [Header("Fixed Buns")]
    public int bottomBunCount = 1;
    public int topBunCount = 1;

    [Header("Ingredient Ranges")]
    public int minCutlets = 1, maxCutlets = 3;
    public int minCheese = 0, maxCheese = 2;
    public int minTomato = 0, maxTomato = 2;
    public int minLettuce = 0, maxLettuce = 2;

    [Header("Gameflow Reference")]
    [Tooltip("Перетягни сюди свій GameflowManager з компонентом gameflow")]
    public gameflow gf;

    void Awake()
    {
        if (gf == null)
            Debug.LogError("OrderGenerator: поле gf (gameflow) не призначене в Inspector!");
    }

    /// <summary>
    /// Очищає попереднє замовлення та створює нове
    /// </summary>
    public void GenerateOrder()
    {
        if (contentPanel == null || orderLinePrefab == null || gf == null)
        {
            Debug.LogError("OrderGenerator: невірно налаштовані поля (contentPanel, orderLinePrefab або gf)!");
            return;
        }

        // 1) Очистка старих рядків
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        // 2) Згенеруємо кількості
        int[] qty = new int[6] {
            bottomBunCount,
            Random.Range(minCutlets, maxCutlets + 1),
            Random.Range(minCheese,  maxCheese  + 1),
            Random.Range(minTomato,  maxTomato  + 1),
            Random.Range(minLettuce, maxLettuce + 1),
            topBunCount
        };

        // 3) Відобразимо їх у UI
        string[] labels = {
            "Bun Bottom", "Cutlet", "Cheese",
            "Tomato", "Lettuce", "Bun Top"
        };
        for (int i = 0; i < labels.Length; i++)
        {
            var line = Instantiate(orderLinePrefab, contentPanel, false);
            line.text = $"{labels[i]}: {qty[i]}";
        }

        // 4) Зібрати код і передати в gameflow
        string codeStr = "";
        foreach (int x in qty) codeStr += x;
        if (int.TryParse(codeStr, out int code))
        {
            gf.SetOrderValue(code);
        }
        else
        {
            Debug.LogError($"OrderGenerator: не вдалося конвертувати '{codeStr}' у число!");
        }
    }
}
