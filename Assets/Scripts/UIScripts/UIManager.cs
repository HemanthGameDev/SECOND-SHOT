using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;


    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }
    
    public void WinUI()
    {
        winUI.SetActive(true);
    }
    public void Retry()
    {
        Debug.Log("RETRY BUTTON PRESSED");

        HideAllUI();

        RoundManager.instance.RestartGame();
    }

    public void PlayAgain()
    {
        HideAllUI();
        RoundManager.instance.RestartGame();
    }

    private void HideAllUI()
    {
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
    }
}
