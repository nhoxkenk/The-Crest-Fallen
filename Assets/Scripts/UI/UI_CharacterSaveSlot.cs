using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CharacterSaveSlot : MonoBehaviour
{
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;
    [SerializeField] private string gameSlotName;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterPlayedTimeText;

    private void OnEnable()
    {
        //LoadSaveSlotFromWriter();
        LoadSaveDataFromSystem();
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

    private void LoadSaveDataFromSystem()
    {
        gameSlotName = SaveLoadSystem.Instance.DecideCharacterFileNameBasedOnSlotBeingUsed(characterSlot);
        int indexSlot = SaveLoadSystem.Instance.characterSlotDatas?.FindIndex(slot => slot != null && slot.FileName != null && slot.FileName.Equals(gameSlotName)) ?? -1;
        if (indexSlot < 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            characterNameText.text = SaveLoadSystem.Instance.characterSlotDatas[indexSlot].playerData.CharacterName;
        }
    }

    public void LoadGameDataFromThisSlot()
    {
        //WorldSaveManager.Instance.currentSlot = characterSlot;
        //WorldSaveManager.Instance.LoadGame();
        SaveLoadSystem.Instance.currentGameDataSlot = characterSlot;
        SaveLoadSystem.Instance.LoadGame(gameSlotName);
    }

    public void SelectThisSlot()
    {
        TitleScreenManager.Instance.AssignToHighlightedSlot(characterSlot);
    }
}
