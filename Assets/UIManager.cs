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
    }

    private void OnDestroy()
    {
        EventManager.OnLevelFailed -= LevelFailedEventHandler;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void LevelFailedEventHandler(object sender, EventArgs args)
    {
        failPanel.SetActive(true);
    }
}
