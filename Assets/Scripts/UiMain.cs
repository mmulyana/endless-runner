using UnityEngine;

public class UiMain : MonoBehaviour
{
    public void SwithMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }

    public void StartGame() => GameManager.instance.startPlayerRun();
}
