using System.Collections;
using UnityEngine;

public class CollisionLocker : MonoBehaviour
{
    public Transform plateSpawnPoint;
    public float delay = 0.1f;      // скільки секунд почекати після удару
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Plate"))
        {
            StartCoroutine(FixAfterLanding(collision));
        }
    }


    private IEnumerator FixAfterLanding(Collision collision)
    {
        yield return new WaitForSeconds(delay);

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // Знаходимо LevelManager і беремо його plateSpawnPoint
        var lm = Object.FindFirstObjectByType<LevelManager>();
        if (lm != null && lm.plateSpawnPoint != null)
        {
            transform.SetParent(lm.plateSpawnPoint, true);
        }
        else
        {
            // як запасний варіант — чіпляємося до того, з чим зіткнулися
            transform.SetParent(collision.transform, true);
        }
    }
}
