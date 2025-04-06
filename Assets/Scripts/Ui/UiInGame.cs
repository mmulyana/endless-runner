using TMPro;
using UnityEngine;

public class UiInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;

    void Start()
    {
        InvokeRepeating("UpdateInfo", 0, .15f);
    }

    private void UpdateInfo()
    {
        coins.text = GameManager.instance.coins.ToString();
    }
}
