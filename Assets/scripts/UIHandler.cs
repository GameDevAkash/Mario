using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;
    public TextMeshProUGUI CoinsCount_Text;
    public Button RewindSameLevelButton;
    public GameObject WinPanel, MainMenuPanel;
    public PlayerMovement mario;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        RewindSameLevelButton.onClick.AddListener(RewindSameLevel);
    }

    public void PlayGame()
    {
        MainMenuPanel.SetActive(false);
        mario.CanMove = true;
    }

    public void RewindSameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
