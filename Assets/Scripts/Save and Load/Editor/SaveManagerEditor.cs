using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveLoadSystem))]
public class SaveManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        SaveLoadSystem saveLoadSystem = (SaveLoadSystem) target;
        string gameName = saveLoadSystem.gameData.FileName;

        DrawDefaultInspector();

        if (GUILayout.Button("New Game"))
        {
            saveLoadSystem.NewGame();
        }

        if (GUILayout.Button("Save game"))
        {
            saveLoadSystem.SaveGame();
        }

        if (GUILayout.Button("Load game"))
        {
            saveLoadSystem.LoadGame(gameName);
        }

        if (GUILayout.Button("Load All"))
        {
            saveLoadSystem.LoadAll();
        }

        if (GUILayout.Button("Delete game"))
        {
            saveLoadSystem.DeleteGame(gameName);
        }
    }
}
