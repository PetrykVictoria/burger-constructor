using UnityEngine;

public class Serveplate : MonoBehaviour
{
    private gameflow gf;

    void Start()
    {
        gf = Object.FindFirstObjectByType<gameflow>();
        if (gf == null)
            Debug.LogError("Gameflow �� �������� � ����!");
    }

    void OnMouseDown()
    {
        Debug.Log("Serveplate: ��� � �������� CheckOrder()");
        gf.CheckOrder();
    }
}
