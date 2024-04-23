using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{

    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    public void StartNewGame()
    {
        WorldSaveManager.Instance.CreateNewGame();
        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene());
    }

    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
    }

    public void CloseLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(true);
        titleScreenLoadMenu.SetActive(false);
    }
}
