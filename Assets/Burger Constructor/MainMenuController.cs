using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject aboutPanel;
    public GameObject levelSelectPanel;

    [Header("Level Select UI")]
    public int totalLevels = 5;
    public Transform levelsContainer;
    public Button levelButtonPrefab;
    public string levelScenePrefix = "Level";

    public static class GameSettings
    {
        public static int startLevel = 0;
    }

    public void OnStartGame()
    {
        GameSettings.startLevel = 0;
        SceneManager.LoadScene("SampleScene");
    }


    public void OnAbout()
    {
        menuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void OnBackFromAbout()
    {
        aboutPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnLevelSelect()
    {
        Debug.Log("OnLevelSelect() called");

        if (levelsContainer == null)
        {
            Debug.LogError("MainMenuController: levelsContainer не призначено!");
            return;
        }
        if (levelButtonPrefab == null)
        {
            Debug.LogError("MainMenuController: levelButtonPrefab не призначено!");
            return;
        }

        menuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);

        // очистка
        foreach (Transform t in levelsContainer)
            Destroy(t.gameObject);

        for (int i = 1; i <= totalLevels; i++)
        {
            var btn = Instantiate(levelButtonPrefab, levelsContainer, false);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + i;
            int index = i - 1; // 0-based індекс
            btn.onClick.AddListener(() =>
            {
                GameSettings.startLevel = index;
                SceneManager.LoadScene("SampleScene");
            });
        }
    
    }


    public void OnBackFromLevelSelect()
    {
        if (levelsContainer != null)
        {
            foreach (Transform t in levelsContainer)
                Destroy(t.gameObject);
        }
        else
        {
            Debug.LogWarning("MainMenuController: levelsContainer не призначено!");
        }

        levelSelectPanel.SetActive(false);
        menuPanel.SetActive(true);
    }


    public void OnQuit()
    {
        Application.Quit();
    }
}
