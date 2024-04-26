using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CharacterSaveSlot : MonoBehaviour
{
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterPlayedTimeText;

    private void OnEnable()
    {
        LoadSaveSlotFromWriter();
    }

    private void LoadSaveSlotFromWriter()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = WorldSaveManager.Instance.DecideCharacterFileNameBasedOnSlotBeingUsed(characterSlot);

        if(characterSlot == CharacterSlot.CharacterSlot_01)
        {
            if (saveFileDataWriter.CheckIfSaveFileExists())
            {
                characterNameText.text = WorldSaveManager.Instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (characterSlot == CharacterSlot.CharacterSlot_02)
        {
            if (saveFileDataWriter.CheckIfSaveFileExists())
            {
                characterNameText.text = WorldSaveManager.Instance.characterSlot02.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (characterSlot == CharacterSlot.CharacterSlot_03)
        {
            if (saveFileDataWriter.CheckIfSaveFileExists())
            {
                characterNameText.text = WorldSaveManager.Instance.characterSlot03.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (characterSlot == CharacterSlot.CharacterSlot_04)
        {
            if (saveFileDataWriter.CheckIfSaveFileExists())
            {
                characterNameText.text = WorldSaveManager.Instance.characterSlot04.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (characterSlot == CharacterSlot.CharacterSlot_05)
        {
            if (saveFileDataWriter.CheckIfSaveFileExists())
            {
                characterNameText.text = WorldSaveManager.Instance.characterSlot05.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (characterSlot == CharacterSlot.CharacterSlot_06)
        {
            if (saveFileDataWriter.CheckIfSaveFileExists())
            {
                characterNameText.text = WorldSaveManager.Instance.characterSlot06.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void LoadGameDataFromThisSlot()
    {
        WorldSaveManager.Instance.currentSlot = characterSlot;
        WorldSaveManager.Instance.LoadGame();
    }

    public void SelectThisSlot()
    {
        TitleScreenManager.Instance.AssignToHighlightedSlot(characterSlot);
    }
}
