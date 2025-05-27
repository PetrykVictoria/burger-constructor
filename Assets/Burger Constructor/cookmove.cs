using System.Collections;
using UnityEngine;

public class CookMove : MonoBehaviour
{
    public float delay = 0.01f;
    private Rigidbody rb;

    [Header("Префаб і точки спавну")]
    [Tooltip("Префаб котлети (із Rigidbody, Collider та цим скриптом)")]
    public GameObject cutletPrefab;
    [Tooltip("Де спавниться сирий шматок (на пані)")]
    public Transform panSpawnPoint;
    [Tooltip("Де спавниться готовий шматок (на тарілці)")]
    public Transform plateSpawnPoint;

    // стани
    private bool hasSpawnedOnPan = false;
    private bool isCooked = false;

    private MeshRenderer meatMat;
    private gameflow gameflow;
    private const int foodValue = 10000;

    private void Awake()
    {
        meatMat = GetComponent<MeshRenderer>();
        gameflow = Object.FindFirstObjectByType<gameflow>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Plate"))
            StartCoroutine(FixAfterLanding(col.transform));
    }

    private IEnumerator FixAfterLanding(Transform plate)
    {
        yield return new WaitForSeconds(delay);

        // зафіксувати фізику
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // зробити дочірнім до тарілки
        transform.SetParent(plate, true);
    }

    private void OnMouseDown()
    {
        // 1) Перший клік: спавнимо пан-клон
        if (!hasSpawnedOnPan)
        {
            var panClone = Instantiate(
                cutletPrefab,
                panSpawnPoint.position,
                panSpawnPoint.rotation
            );
            var cook = panClone.GetComponent<CookMove>();
            cook.hasSpawnedOnPan = true;
            cook.cutletPrefab = this.cutletPrefab;
            cook.panSpawnPoint = this.panSpawnPoint;
            cook.plateSpawnPoint = this.plateSpawnPoint;
            cook.StartCooking();
            return;
        }

        // 2) Якщо пан-клон ще не готовий
        if (!isCooked)
        {
            Debug.Log("Котлета ще готується...");
            return;
        }

        // 3) Другий клік: **не Instantiate префабу**, а просто перемістити цей пан-клон на тарілку
        transform.SetParent(plateSpawnPoint);
        transform.position = plateSpawnPoint.position;
        transform.rotation = plateSpawnPoint.rotation;

        // знімаємо скрипт, щоб це вже був «чистий» шматок
        // другий клік, готовий пан-клон → тарілка
        // замість Destroy(gameObject) тут ми працюємо з пан-клоном:
        if (hasSpawnedOnPan && isCooked)
        {
            // 1) замість жорсткого Instantiate(prefab...), беремо сам пан-клон:
            GameObject plateCutlet = this.gameObject;

            // 2) настроюємо фізику для падіння:
            var rb = plateCutlet.GetComponent<Rigidbody>();
            if (rb == null) rb = plateCutlet.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            // 3) персоналізуємо його положення трохи вище тарілки:
            plateCutlet.transform.position = plateSpawnPoint.position + Vector3.up * 0.2f;
            plateCutlet.transform.rotation = plateSpawnPoint.rotation;

            // 4) додаємо скрипт фіксації:
            plateCutlet.AddComponent<CollisionLocker>();

            // 5) забираємо цей CookMove, бо більше не потрібен:
            Destroy(this);

            // 6) оновлюємо рахунок (можна одразу, або в OnCollisionEnter теж, якщо треба):
            gameflow.plateValue += foodValue;
            Debug.Log($"Plate: {gameflow.plateValue} / Order: {gameflow.orderValue}");
        }
    }


    /// <summary>
    /// Запускає таймер приготування в пан-клоні
    /// </summary>
    public void StartCooking()
    {
        StartCoroutine(CookTimer());
    }

    private IEnumerator CookTimer()
    {
        yield return new WaitForSeconds(5f);
        isCooked = true;
        meatMat.material.color = new Color(0.36f, 0.25f, 0.20f);
    }
}
