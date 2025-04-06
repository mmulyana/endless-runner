using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UiMain ui;
    public Player player;

    public int coins;

    private void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }


    private void Awake()
    {
        instance = this;
    }

    public void startPlayerRun() => player.runBegin = true;

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        SaveInfo();
    }

    public void SaveInfo()
    {
        int savedCoins = PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", savedCoins + coins);
    }

    public void EndGame()
    {
        SaveInfo();
        ui.showEndUI();
    }
}
