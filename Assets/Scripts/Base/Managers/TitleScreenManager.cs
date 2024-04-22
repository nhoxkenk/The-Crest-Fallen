using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame()
    {
        WorldSaveManager.Instance.CreateNewGame();
        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene());
    }
}
