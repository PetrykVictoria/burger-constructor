using UnityEngine;

[System.Serializable]
public class LevelData
{
    [Tooltip("Назва рівня для відображення")]
    public string levelName;

    [Tooltip("Ліміт часу (в секундах)")]
    public float timeLimit = 60f;

    [Header("Діапазони кількостей інгредієнтів")]
    public int minCutlets = 1, maxCutlets = 3;
    public int minCheese = 0, maxCheese = 2;
    public int minTomato = 0, maxTomato = 2;
    public int minLettuce = 0, maxLettuce = 2;
}
