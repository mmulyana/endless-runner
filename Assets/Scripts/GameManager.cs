using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;

    public int coins;
    

    private void Awake()
    {
        instance = this;
    }

    public void startPlayerRun() => player.runBegin = true;

    public void RestartLevel() => SceneManager.LoadScene(0);
}
