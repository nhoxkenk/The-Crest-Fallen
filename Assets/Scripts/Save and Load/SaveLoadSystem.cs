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

    public GameData gameData;

    private IDataService dataService;

    protected override void Awake()
    {
        base.Awake();

        dataService = new FileDataService(new JsonSerializer());
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoad;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoad;

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Bind<TestHeroScript, PlayerData>(gameData.playerData);
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
        StartCoroutine(LoadWorldScene());
        return;
    }

    public void SaveGame()
    {
        dataService.Save(gameData);
    }

    public void LoadGame(string gameName)
    {
        gameData = dataService.Load(gameName);

        if (string.IsNullOrWhiteSpace(gameName))
        {
            //something fire off here
        }
        StartCoroutine(LoadWorldScene());
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
}
