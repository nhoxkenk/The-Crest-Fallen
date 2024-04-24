using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveManager : Singleton<WorldSaveManager>
{
    [Header("Save/Load")]
    [SerializeField] private bool isSaved;
    [SerializeField] private bool isLoaded;

    [SerializeField] private int worldSceneIndex = 1;
    public int WorldSceneIndex { get { return worldSceneIndex; } }

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentSlot;
    public CharacterSaveData currentCharacterData;
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

    public void AttempToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;
        //Check here to see if we can create new save file (existing file)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        
        if (!saveFileDataWriter.CheckIfSaveFileExists())
        {
            currentSlot = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_02);

        if (!saveFileDataWriter.CheckIfSaveFileExists())
        {
            currentSlot = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        TitleScreenManager.Instance.DisplayNoFreeCharacterSlotPopUp();

        //saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        //if (saveFileDataWriter.CheckIfSaveFileExists())
        //{
        //    currentSlot = CharacterSlot.CharacterSlot_03;
        //    currentCharacterData = new CharacterSaveData();
        //    StartCoroutine(LoadWorldScene());
        //    return;
        //}

        //saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_04);

        //if (saveFileDataWriter.CheckIfSaveFileExists())
        //{
        //    currentSlot = CharacterSlot.CharacterSlot_04;
        //    currentCharacterData = new CharacterSaveData();
        //    StartCoroutine(LoadWorldScene());
        //    return;
        //}

        //saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_05);

        //if (saveFileDataWriter.CheckIfSaveFileExists())
        //{
        //    currentSlot = CharacterSlot.CharacterSlot_05;
        //    currentCharacterData = new CharacterSaveData();
        //    StartCoroutine(LoadWorldScene());
        //    return;
        //}

        //saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot.CharacterSlot_06);

        //if (saveFileDataWriter.CheckIfSaveFileExists())
        //{
        //    currentSlot = CharacterSlot.CharacterSlot_06;
        //    currentCharacterData = new CharacterSaveData();
        //    StartCoroutine(LoadWorldScene());
        //    return;
        //}

    }

    public void LoadGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveFileDataPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        currentCharacterData = saveFileDataWriter.LoadCharacterSaveFile();

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

        //PlayerManager.Instance.LoadGameDataToCurrentCharacterData(ref currentCharacterData);

        yield return null;
    }
}
