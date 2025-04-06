using TMPro;
using UnityEngine;

public class UIEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCoins;

    void Start()
    {
        int myCoins = PlayerPrefs.GetInt("Coins");
        textCoins.text = "coin: " + myCoins.ToString();

        Time.timeScale = 0;
    }
}
