using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : Singleton<TitleScreenManager>
{
    [Header("Menus")]
    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;

    [Header("Pop Up")]
    [SerializeField] private GameObject titleScreenNoCharacterSlot;
    [SerializeField] private GameObject deleteCharacterSlotPopup;

    [Header("Character highlighted Slot")]
    [SerializeField] private CharacterSlot highlightedSlot;

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

    public void AssignToHighlightedSlot(CharacterSlot slot)
    {
        highlightedSlot = slot;
    }

    public void NoSlotHighlighted()
    {
        highlightedSlot = CharacterSlot.NoSlot;
    }

    public void AttempToDeleteSlot()
    {
        if(highlightedSlot != CharacterSlot.NoSlot)
        {
            deleteCharacterSlotPopup.SetActive(true);
            titleScreenLoadMenu.SetActive(false);
        }
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopup.SetActive(false);
        WorldSaveManager.Instance.DeleteGame(highlightedSlot);
        titleScreenLoadMenu.SetActive(true);
    }

    public void CloseDeleteCharacterPopup()
    {
        deleteCharacterSlotPopup.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
    }
}
