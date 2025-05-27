using UnityEngine;

public class Clickplace : MonoBehaviour
{
    [Tooltip("Префаб інгредієнта для клонування")]
    public GameObject clonePrefab;
    [Tooltip("Точка, куди спавниться клон")]
    public Transform spawnPoint;
    [Tooltip("Цінність цього інгредієнта")]
    public int foodValue;

    private gameflow gf;

    void Awake()
    {
        // Знаходимо єдиний екземпляр gameflow сучасним методом
        gf = Object.FindFirstObjectByType<gameflow>();
        if (gf == null)
            Debug.LogError("Clickplace: не знайдено gameflow у сцені!");
    }

    private void OnMouseDown()
    {
        // 1) Інстанціюємо клон на позиції spawnPoint
        GameObject spawned = Instantiate(
            clonePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 2) Парентимо його до spawnPoint, щоб можна було легко видаляти
        spawned.transform.SetParent(spawnPoint, worldPositionStays: true);

        // 3) Додаємо CollisionLocker (якщо потрібна фізика)
        spawned.AddComponent<CollisionLocker>();

        // 4) Оновлюємо значення тарілки
        gf.plateValue += foodValue;

        Debug.Log($"Plate: {gf.plateValue}, Order: {gf.orderValue}");
    }

}
