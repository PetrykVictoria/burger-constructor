using UnityEngine;

public class Clickplace : MonoBehaviour
{
    [Tooltip("������ �����䳺��� ��� ����������")]
    public GameObject clonePrefab;
    [Tooltip("�����, ���� ���������� ����")]
    public Transform spawnPoint;
    [Tooltip("ֳ����� ����� �����䳺���")]
    public int foodValue;

    private gameflow gf;

    void Awake()
    {
        // ��������� ������ ��������� gameflow �������� �������
        gf = Object.FindFirstObjectByType<gameflow>();
        if (gf == null)
            Debug.LogError("Clickplace: �� �������� gameflow � ����!");
    }

    private void OnMouseDown()
    {
        // 1) ������������ ���� �� ������� spawnPoint
        GameObject spawned = Instantiate(
            clonePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 2) ��������� ���� �� spawnPoint, ��� ����� ���� ����� ��������
        spawned.transform.SetParent(spawnPoint, worldPositionStays: true);

        // 3) ������ CollisionLocker (���� ������� ������)
        spawned.AddComponent<CollisionLocker>();

        // 4) ��������� �������� ������
        gf.plateValue += foodValue;

        Debug.Log($"Plate: {gf.plateValue}, Order: {gf.orderValue}");
    }

}
