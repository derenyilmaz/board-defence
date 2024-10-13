using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        EventManager.OnLevelFailed += LevelFailedEventHandler;
        EventManager.OnLevelWon += LevelWonEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnLevelFailed -= LevelFailedEventHandler;
        EventManager.OnLevelWon -= LevelWonEventHandler;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void LevelFailedEventHandler(object sender, EventArgs args)
    {
        failPanel.SetActive(true);
    }
    
    private void LevelWonEventHandler(object sender, EventArgs args)
    {
        var currentLevel = PlayerPrefs.GetInt(Constants.LevelIndexKey, defaultValue: 0);
        PlayerPrefs.SetInt(Constants.LevelIndexKey, currentLevel + 1);
        
        winPanel.SetActive(true);
    }
}
