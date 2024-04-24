using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : Singleton<TitleScreenManager>
{

    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    [Header("Pop Up")]
    [SerializeField] private GameObject titleScreenNoCharacterSlot;

    public void StartNewGame()
    {
        WorldSaveManager.Instance.AttempToCreateNewGame();
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

    public void DisplayNoFreeCharacterSlotPopUp()
    {
        titleScreenNoCharacterSlot.SetActive(true);
    }

    public void HideNoFreeCharacterSlotPopUp()
    {
        titleScreenNoCharacterSlot.SetActive(false);
    }
}
