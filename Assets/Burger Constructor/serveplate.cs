using UnityEngine;

public class Serveplate : MonoBehaviour
{
    private gameflow gf;

    void Start()
    {
        gf = Object.FindFirstObjectByType<gameflow>();
        if (gf == null)
            Debug.LogError("Gameflow не знайдено в сцен≥!");
    }

    void OnMouseDown()
    {
        Debug.Log("Serveplate: кл≥к Ч запускаю CheckOrder()");
        gf.CheckOrder();
    }
}
