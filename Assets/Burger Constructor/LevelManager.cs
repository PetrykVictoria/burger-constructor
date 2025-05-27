using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static MainMenuController;

public class LevelManager : MonoBehaviour
{
    [Header("����� ������ �����䳺���")]
    [Tooltip("������ ��� Transform, ���� ����������� �������� �����䳺���")]
    public Transform[] ingredientSpawnPoints;

    [Header("����� �������� ������")]
    [Tooltip("���������, �� ������ �� �������� �� ������")]
    public Transform plateSpawnPoint;

    [Header("������������ ����")]
    public LevelData[] levels;

    [Header("UI")]
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI timerText;
    public Button serveButton;

    [Header("End Level UI")]
    public GameObject endLevelPanel;
    public TextMeshProUGUI endMessageText;
    public Button nextLevelButton;
    public Button mainMenuButton;

    [Header("����������")]
    public OrderGenerator orderGenerator;
    public gameflow gf;

    int currentLevel;
    float timeRemaining;
    bool isRunning;
    bool lastSuccess;

    void Awake()
    {
        if (endLevelPanel != null)
            endLevelPanel.SetActive(false);

        int lvl = GameSettings.startLevel;
        GameSettings.startLevel = 0;
        StartLevel(lvl);
    }

    void Start()
    {
        serveButton?.onClick.AddListener(OnServePressed);
        nextLevelButton?.onClick.AddListener(OnNextPressed);
        mainMenuButton?.onClick.AddListener(OnMainMenuPressed);
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f) timeRemaining = 0f;
        timerText.text = FormatTime(timeRemaining);

        if (timeRemaining <= 0f)
            EndLevel(false);
    }

    void StartLevel(int index)
    {
        // 1) ������� ������
        if (plateSpawnPoint != null)
            ClearChildren(plateSpawnPoint);
        else
            Debug.LogWarning("LevelManager: plateSpawnPoint �� ����������!");

        // 2) ������� �� ����� ������ �����䳺���
        foreach (var sp in ingredientSpawnPoints)
        {
            if (sp != null)
                ClearChildren(sp);
            else
                Debug.LogWarning("LevelManager: ���� �� ingredientSpawnPoints �� ����������!");
        }

        // 3) ���������� �������� �������
        if (index < 0 || index >= levels.Length)
        {
            Debug.Log("�� ��� ������� ��� ������ �� ������!");
            isRunning = false;
            return;
        }

        currentLevel = index;
        var data = levels[index];

        // 4) ³��������� ����� �� ������
        levelNameText.text = data.levelName;
        timeRemaining = data.timeLimit;
        timerText.text = FormatTime(timeRemaining);

        // 5) �������� ����������
        orderGenerator.minCutlets = data.minCutlets;
        orderGenerator.maxCutlets = data.maxCutlets;
        orderGenerator.minCheese = data.minCheese;
        orderGenerator.maxCheese = data.maxCheese;
        orderGenerator.minTomato = data.minTomato;
        orderGenerator.maxTomato = data.maxTomato;
        orderGenerator.minLettuce = data.minLettuce;
        orderGenerator.maxLettuce = data.maxLettuce;
        orderGenerator.GenerateOrder();

        // 6) ������� �������� ������
        gf.plateValue = 0;
        gf.UpdateUI();

        // 7) ��������� �����
        isRunning = true;
    }

    private void ClearChildren(Transform container)
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    void OnServePressed()
    {
        lastSuccess = (gf.plateValue == gf.orderValue);
        EndLevel(lastSuccess);
    }

    void EndLevel(bool success)
    {
        isRunning = false;
        if (endLevelPanel != null)
            endLevelPanel.SetActive(true);

        endMessageText.text = success ? "Level Complete!" : "Level Failed!";
        nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text =
            success ? "Next Level" : "Restart";
    }

    void OnNextPressed()
    {
        endLevelPanel.SetActive(false);
        if (lastSuccess)
            StartLevel(currentLevel + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        return $"{m:00}:{s:00}";
    }
}
