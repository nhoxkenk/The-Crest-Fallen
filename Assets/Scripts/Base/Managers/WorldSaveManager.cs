using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveManager : Singleton<WorldSaveManager>
{

    [SerializeField] private PlayerManager playerManager;

    [Header("Save/Load")]
    [SerializeField] private bool isSaved;
    [SerializeField] private bool isLoaded;

    [SerializeField] private int worldSceneIndex = 1;
    public int WorldSceneIndex { get { return worldSceneIndex; } }

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    [SerializeField] private CharacterSlot currentSlot;
    [SerializeField] private CharacterSaveData currentCharacterData;
    public string saveFileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;

    private void Update()
    {
        if (isSaved)
        {
            isSaved = false;
            SaveGame();
        }

        if(isLoaded)
        {
            isLoaded = false;
            LoadGame();
        }
    }

    private void DecideCharacterFileNameBasedOnSlotBeingUsed()
    {
        saveFileName = "characterSlot_" + ((int)currentSlot).ToString("00");
    }

    public void CreateNewGame()
    {
        DecideCharacterFileNameBasedOnSlotBeingUsed();

        currentCharacterData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        DecideCharacterFileNameBasedOnSlotBeingUsed();

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        currentCharacterData = saveFileDataWriter.LoadCharacterSaveFile();
        Debug.Log(playerManager.transform.position);
        playerManager.LoadGameDataToCurrentCharacterData(ref currentCharacterData);
        Debug.Log(playerManager.transform.position);
        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        DecideCharacterFileNameBasedOnSlotBeingUsed();

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        PlayerManager.Instance.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
        yield return null;
    }
}
