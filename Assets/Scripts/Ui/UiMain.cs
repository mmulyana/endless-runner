using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMain : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endMenu;

    private void Start()
    {
        Time.timeScale = 1;
        SwithMenuTo(mainMenu);

        coins.text = GameManager.instance.coins.ToString();
    }

    public void SwithMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }

    public void StartGame() => GameManager.instance.startPlayerRun();

    public void PauseGame()
    {
        if(gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
            return;
        }

        Time.timeScale = 0;
        gamePaused = true;
    }

    public void showEndUI()
    {
        SwithMenuTo(endMenu);
    }

    public void StartFromStart() => SceneManager.LoadScene(0);
}