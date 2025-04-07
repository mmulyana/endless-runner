using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMain : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;

    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endMenu;

    [Header("Volume Slider info")]
    [SerializeField] private UIVolumeSlider[] slider;
    [SerializeField] private Image mutedIcon;

    private void Start()
    {
        for(int i = 0; i < slider.Length; i++)
        {
            slider[i].SetupSlider();
        }

        Time.timeScale = 1;
        SwithMenuTo(mainMenu);

        coins.text = GameManager.instance.coins.ToString();
    }

    public void SwithMenuTo(GameObject uiMenu)
    {
        AudioManager.instance.PlaySFX(8);
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

    public void MutedButton()
    {
        gameMuted = !gameMuted;

        if(gameMuted)
        {
            AudioListener.volume = 0;
        } else
        {
            AudioListener.volume = 1;
        }
    }
}