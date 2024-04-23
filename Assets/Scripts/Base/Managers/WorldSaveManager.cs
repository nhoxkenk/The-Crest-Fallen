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
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;

    private void Start()
    {
        LoadAllCharacterSaveFiles();
    }

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

    public string DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot characterSlot)
    {
        return "characterSlot_" + ((int)characterSlot).ToString("00");
    }

    public void CreateNewGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentSlot);

        currentCharacterData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        currentCharacterData = saveFileDataWriter.LoadCharacterSaveFile();
        Debug.Log(playerManager.transform.position);
        playerManager.LoadGameDataToCurrentCharacterData(ref currentCharacterData);
        Debug.Log(playerManager.transform.position);
        StartCoroutine(LoadWorldScene());
    }

    public void LoadAllCharacterSaveFiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_01);  
        characterSlot01 = saveFileDataWriter.LoadCharacterSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadCharacterSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadCharacterSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadCharacterSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadCharacterSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWriter.LoadCharacterSaveFile();
    }

    public void SaveGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentSlot);

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
