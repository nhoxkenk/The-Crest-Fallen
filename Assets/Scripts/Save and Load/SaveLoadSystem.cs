using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISaveable
{
    public string Id { get; set; }
}

public interface IBind<TData> where TData : ISaveable
{
    public string Id { get; set; }
    public void Bind(TData data);
}

public class SaveLoadSystem : Singleton<SaveLoadSystem>
{
    [Header("Scene Index")]
    [SerializeField] private int worldSceneIndex = 1;
    public int WorldSceneIndex { get { return worldSceneIndex; } }

    [Header("Current Game Data")]
    public GameData gameData;
    public CharacterSlot currentGameDataSlot;

    [Header("All Character Slot Datas")]
    public List<GameData> characterSlotDatas;

    private IDataService dataService;

    protected override void Awake()
    {
        base.Awake();

        dataService = new FileDataService(new JsonSerializer());
    }

    //private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoad;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoad;

    private void Start()
    {
        LoadAll();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MenuScene") return;
        Bind<PlayerSaveData, PlayerData>(gameData.playerData);
    }

    //Handle One entity, often is main character
    private void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
    {
        var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
        if (entity != null)
        {
            if (data == null)
            {
                data = new TData { Id = entity.Id };
            }

            entity.Bind(data);
        }
    }

    //Handle multiple entity, can be use when Save and load data of inventory, enemies, etc...
    private void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
    {
        var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

        foreach(var entity in entities)
        {
            var data = datas.FirstOrDefault(d => d.Id == entity.Id);
            if (data == null)
            {
                data = new TData { Id = entity.Id };
                datas.Add(data);
            }
            entity.Bind(data);
        }
        
    }

    public void NewGame()
    {
        int emptySlot = characterSlotDatas.FindIndex(slot => slot.FileName == null || slot.FileName.Equals(""));
        if (emptySlot == -1)
        {
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotPopUp();
            return;
        }
        currentGameDataSlot = (CharacterSlot) emptySlot + 1;

        gameData = new GameData {
            FileName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentGameDataSlot),
            playerData = new PlayerData()
        };

        StartCoroutine(LoadWorldScene());
        return;
    }

    public void SaveGame()
    {
        gameData.FileName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentGameDataSlot);
        dataService.Save(gameData);
    }

    public void LoadGame(string gameName)
    {
        gameName = DecideCharacterFileNameBasedOnSlotBeingUsed(currentGameDataSlot);
        gameData = dataService.Load(gameName);

        if (string.IsNullOrWhiteSpace(gameName))
        {
            //something fire off here
            Debug.Log("Error");
        }
        StartCoroutine(LoadWorldScene());
    }

    public void LoadAll()
    {
        for (int i = 0; i < characterSlotDatas.Count; i++)
        {
            GameData gameData = new GameData();
            characterSlotDatas[i] = gameData;
        }

        foreach (string fileName in dataService.ListSaves())
        {
            string slotNumber = fileName.Substring(fileName.LastIndexOf('_') + 1);
            int indexSlot = int.Parse(slotNumber) - 1;
            if (indexSlot >= 0 && indexSlot < characterSlotDatas.Count)
            {
                GameData tempGameData = dataService.Load(fileName);
                if (tempGameData != null)
                {
                    characterSlotDatas[indexSlot] = tempGameData;
                }
            }
            //if (tempGameData != null)
            //{
            //    var emptySlot = characterSlotDatas.FindIndex(slot => slot.FileName == null || slot.FileName.Equals(""));
            //    if (emptySlot != -1)
            //    {
            //        characterSlotDatas[emptySlot] = tempGameData;
            //    }
            //}
        }
    }

    public void DeleteGame(string gameName)
    {
        dataService.Delete(gameName);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);

        yield return null;
    }

    public string DecideCharacterFileNameBasedOnSlotBeingUsed(CharacterSlot characterSlot)
    {
        return "characterSlot_" + ((int)characterSlot).ToString("00");
    }
}
